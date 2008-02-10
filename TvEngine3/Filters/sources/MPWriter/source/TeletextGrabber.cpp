/* 
*	Copyright (C) 2006-2008 Team MediaPortal
*	http://www.team-mediaportal.com
*
*  This Program is free software; you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation; either version 2, or (at your option)
*  any later version.
*   
*  This Program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
*  GNU General Public License for more details.
*   
*  You should have received a copy of the GNU General Public License
*  along with GNU Make; see the file COPYING.  If not, write to
*  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
*  http://www.gnu.org/copyleft/gpl.html
*
*/

#pragma warning(disable : 4995)
#include <windows.h>
#include <commdlg.h>
#include <bdatypes.h>
#include <time.h>
#include <streams.h>
#include <initguid.h>

#include "teletextgrabber.h"


extern void LogDebug(const char *fmt, ...) ;

// table to invert bit ordering of a byte
unsigned char invtab[256] =
{
	0x00, 0x80, 0x40, 0xc0, 0x20, 0xa0, 0x60, 0xe0, 0x10, 0x90, 0x50, 0xd0, 0x30, 0xb0, 0x70, 0xf0, 
	0x08, 0x88, 0x48, 0xc8, 0x28, 0xa8, 0x68, 0xe8, 0x18, 0x98, 0x58, 0xd8, 0x38, 0xb8, 0x78, 0xf8, 
	0x04, 0x84, 0x44, 0xc4, 0x24, 0xa4, 0x64, 0xe4, 0x14, 0x94, 0x54, 0xd4, 0x34, 0xb4, 0x74, 0xf4, 
	0x0c, 0x8c, 0x4c, 0xcc, 0x2c, 0xac, 0x6c, 0xec, 0x1c, 0x9c, 0x5c, 0xdc, 0x3c, 0xbc, 0x7c, 0xfc, 
	0x02, 0x82, 0x42, 0xc2, 0x22, 0xa2, 0x62, 0xe2, 0x12, 0x92, 0x52, 0xd2, 0x32, 0xb2, 0x72, 0xf2, 
	0x0a, 0x8a, 0x4a, 0xca, 0x2a, 0xaa, 0x6a, 0xea, 0x1a, 0x9a, 0x5a, 0xda, 0x3a, 0xba, 0x7a, 0xfa, 
	0x06, 0x86, 0x46, 0xc6, 0x26, 0xa6, 0x66, 0xe6, 0x16, 0x96, 0x56, 0xd6, 0x36, 0xb6, 0x76, 0xf6, 
	0x0e, 0x8e, 0x4e, 0xce, 0x2e, 0xae, 0x6e, 0xee, 0x1e, 0x9e, 0x5e, 0xde, 0x3e, 0xbe, 0x7e, 0xfe, 
	0x01, 0x81, 0x41, 0xc1, 0x21, 0xa1, 0x61, 0xe1, 0x11, 0x91, 0x51, 0xd1, 0x31, 0xb1, 0x71, 0xf1, 
	0x09, 0x89, 0x49, 0xc9, 0x29, 0xa9, 0x69, 0xe9, 0x19, 0x99, 0x59, 0xd9, 0x39, 0xb9, 0x79, 0xf9, 
	0x05, 0x85, 0x45, 0xc5, 0x25, 0xa5, 0x65, 0xe5, 0x15, 0x95, 0x55, 0xd5, 0x35, 0xb5, 0x75, 0xf5, 
	0x0d, 0x8d, 0x4d, 0xcd, 0x2d, 0xad, 0x6d, 0xed, 0x1d, 0x9d, 0x5d, 0xdd, 0x3d, 0xbd, 0x7d, 0xfd, 
	0x03, 0x83, 0x43, 0xc3, 0x23, 0xa3, 0x63, 0xe3, 0x13, 0x93, 0x53, 0xd3, 0x33, 0xb3, 0x73, 0xf3, 
	0x0b, 0x8b, 0x4b, 0xcb, 0x2b, 0xab, 0x6b, 0xeb, 0x1b, 0x9b, 0x5b, 0xdb, 0x3b, 0xbb, 0x7b, 0xfb, 
	0x07, 0x87, 0x47, 0xc7, 0x27, 0xa7, 0x67, 0xe7, 0x17, 0x97, 0x57, 0xd7, 0x37, 0xb7, 0x77, 0xf7, 
	0x0f, 0x8f, 0x4f, 0xcf, 0x2f, 0xaf, 0x6f, 0xef, 0x1f, 0x9f, 0x5f, 0xdf, 0x3f, 0xbf, 0x7f, 0xff, 
};


CTeletextGrabber::CTeletextGrabber(){
	LogDebug("CTeletextGrabber::ctor()");
	m_iCurrentTsPacketCounter=0;
	m_iTsPacketCount=0;
	m_iBufferPos=0;
	m_bRunning=FALSE;
	m_pCallback=NULL;
	m_pBufferTemp = new byte[20000];
	m_pBuffer = new byte[20000];
	m_pInvData = new byte[20000];
	m_pTsResultBuffer = new byte[COLLECT_TS_PACKETS*TS_PACKET_LENGTH+5];
	m_pCurrentTsPacket = new byte[TS_PACKET_LENGTH+5];
	// Fake TsHeader - Byte 0 must be 0x47
	m_pCurrentTsPacket[TSHEADER_BYTE1_OFFSET] = TSHEADER_BYTE1;
	// 0x80 & Byte-1 ==0 -- otherwise Transport error
	m_pCurrentTsPacket[TSHEADER_BYTE2_OFFSET] = TSHEADER_BYTE2;
	// Byte-2 and Byte-3 are not checked
	m_pCurrentTsPacket[TSHEADER_BYTE3_OFFSET] = TSHEADER_BYTE3;
	m_pCurrentTsPacket[TSHEADER_BYTE4_OFFSET] = TSHEADER_BYTE4;
	// Fake PES Informations
	for(int i=0;i<NUMBER_OF_TXT_LINES_IN_TS;i++){
		m_pCurrentTsPacket[DATA_UNIT_OFFSET+(i*DATAPACKET_LENGTH)] = DATA_UNIT;
		m_pCurrentTsPacket[DATA_UNIT_LENGTH_OFFSET+(i*DATAPACKET_LENGTH)] = DATA_UNIT_LENGTH;
		m_pCurrentTsPacket[DATAFIELD_HEADER_OFFSET+(i*DATAPACKET_LENGTH)] = DATAFIELD_HEADER;
		m_pCurrentTsPacket[FRAMING_CODE_OFFSET+(i*DATAPACKET_LENGTH)] = FRAMING_CODE;
	}
}
CTeletextGrabber::~CTeletextGrabber(void)
{
	LogDebug("CTeletextGrabber::dtor()");
	delete[] m_pBuffer;
	delete[] m_pBufferTemp;
	delete[] m_pCurrentTsPacket;
	delete[] m_pTsResultBuffer;
	delete[] m_pInvData;
}


void CTeletextGrabber::SetCallBack( IAnalogTeletextCallBack* callback)
{
	LogDebug("CTeletextGrabber::SetCallBack - %x", callback);
	m_pCallback=callback;
}

void CTeletextGrabber::Start( )
{
	LogDebug("CTeletextGrabber::Start()");
	m_iCurrentTsPacketCounter=0;
	m_iTsPacketCount=0;
	m_iBufferPos=0;
	m_bRunning=true;
}

void CTeletextGrabber::Stop( )
{
	LogDebug("CTeletextGrabber::Stop()");
	m_bRunning=false;
}

void CTeletextGrabber::OnSampleReceived(byte* sampleData, int sampleLen)
{
	try{
		if (!m_bRunning) return;
		if (m_pCallback==NULL) return;
		for(int i=0;i<sampleLen;i++){
			m_pInvData[i] = invtab[sampleData[i]];
		}
		memcpy(&m_pBuffer[m_iBufferPos], m_pInvData,sampleLen);
		m_iBufferPos += sampleLen;
		// Is at least one line in the buffer?
		while(m_iBufferPos>VBI_LINE_LENGTH){
			// Copy data in temporary TsPacket
			if(!(m_pBuffer[0] == 0 && m_pBuffer[1] == 0 && m_pBuffer[2] ==0 && m_pBuffer[3] ==0 && m_pBuffer[4] ==0)){	
				memcpy(&m_pCurrentTsPacket[TXT_LINE_OFFSET+(DATAPACKET_LENGTH*m_iCurrentTsPacketCounter)],m_pBuffer,TXT_LINE_LENGTH);
				m_iCurrentTsPacketCounter++;
				// Ts Packet is full, now copy into buffer
				if(m_iCurrentTsPacketCounter>=NUMBER_OF_TXT_LINES_IN_TS){
					m_iCurrentTsPacketCounter=0;
					memcpy(&m_pTsResultBuffer[m_iTsPacketCount*TS_PACKET_LENGTH],m_pCurrentTsPacket,TS_PACKET_LENGTH);
					m_iTsPacketCount++;
				}
				// Have we collected 25 Ts Packet then callback
				if (m_iTsPacketCount>= COLLECT_TS_PACKETS)
				{
					m_pCallback->OnTeletextReceived(m_pTsResultBuffer, COLLECT_TS_PACKETS);
					m_iTsPacketCount=0;
				}
			}
			// Now clean up the buffer
			m_iBufferPos -=VBI_LINE_LENGTH;
			memcpy(m_pBufferTemp,&m_pBuffer[VBI_LINE_LENGTH],m_iBufferPos);
			memcpy(m_pBuffer,m_pBufferTemp,m_iBufferPos);
		}
	}catch (...) {
		LogDebug("CTELETEXTGRABBER:: ERROR");
	}
}
