#region Copyright (C) 2007-2008 Team MediaPortal

/*
    Copyright (C) 2007-2008 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.Drawing;
using MediaPortal.Core;
using MediaPortal.Presentation.Properties;
using MediaPortal.Control.InputManager;
using Presentation.SkinEngine.Controls.Brushes;
using SlimDX;
using Font = Presentation.SkinEngine.Fonts.Font;
using FontRender = Presentation.SkinEngine.Fonts.FontRender;
using FontBufferAsset = Presentation.SkinEngine.Fonts.FontBufferAsset;
using FontManager = Presentation.SkinEngine.Fonts.FontManager;
using MediaPortal.Utilities.DeepCopy;
using Presentation.SkinEngine.SkinManagement;

namespace Presentation.SkinEngine.Controls.Visuals
{
  public class TextBox : Control
  {
    #region Private fields

    Property _caretIndexProperty;
    Property _textProperty;
    Property _textWrappingProperty;
    Property _colorProperty;
    Property _fontProperty;
    FontBufferAsset _asset;
    FontRender _renderer;
    bool _update = false;

    // If we are editing the text.
    bool _editText = false; 

    #endregion

    #region Ctor

    public TextBox()
    {
      Init();
      Attach();
    }

    void Init()
    {
      _caretIndexProperty = new Property(typeof(int), 0);
      _textProperty = new Property(typeof(string), "");
      _colorProperty = new Property(typeof(Color), Color.Black);
      _fontProperty = new Property(typeof(string), "font13");
      _textWrappingProperty = new Property(typeof(string), "");

      // Yes, we can have focus
      Focusable = true;

      // Set-up default fill
      SolidColorBrush background = new SolidColorBrush();
      background.Color = Color.White;
      Background = background;

      // Set-up default border
      SolidColorBrush border = new SolidColorBrush();
      border.Color = Color.Black;
      BorderBrush = border;

      HorizontalAlignment = HorizontalAlignmentEnum.Left;
    }

    void Attach()
    {
      _fontProperty.Attach(OnFontChanged);
      _textProperty.Attach(OnTextChanged);
      _colorProperty.Attach(OnColorChanged);
    }

    void Detach()
    {
      _fontProperty.Detach(OnFontChanged);
      _textProperty.Detach(OnTextChanged);
      _colorProperty.Detach(OnColorChanged);
    }

    public override void DeepCopy(IDeepCopyable source, ICopyManager copyManager)
    {
      Detach();
      base.DeepCopy(source, copyManager);
      TextBox t = source as TextBox;
      Text = copyManager.GetCopy(t.Text);
      Color = copyManager.GetCopy(t.Color);
      Font = copyManager.GetCopy(t.Font);
      TextWrapping = copyManager.GetCopy(t.TextWrapping);
      CaretIndex = copyManager.GetCopy(t.CaretIndex);
      Attach();
    }

    #endregion

    void OnColorChanged(Property prop)
    {
      _update = true;
      if (Window != null) Window.Invalidate(this);
    }

    void OnTextChanged(Property prop)
    {
      _update = true;

      // The skin is setting the text, also update the caret
      if (!_editText)
      {
        CaretIndex = Text.Length;
      }
      if (Window != null) 
        Window.Invalidate(this);
    }

    void OnFontChanged(Property prop)
    {
      if (_asset != null)
      {
        _asset.Free(true);
        ContentManager.Remove(_asset);
      }

      _asset = null;
      _update = true;
      if (Window != null) Window.Invalidate(this);
    }

    // We need to override this one, so we can subscribe to raw data.
    public override bool HasFocus
    {
      get { return base.HasFocus; }
      set
      {
        base.HasFocus = value;
        IInputManager manager = ServiceScope.Get<IInputManager>();
        
        // We now have focus, so set that we need raw data
        if (value == true)
        {
          manager.NeedRawKeyData = true;
        }
        else
        {
          manager.NeedRawKeyData = false;
        }
      }
    }

    public Property CaretIndexProperty
    {
      get { return _caretIndexProperty; }
    }

    public int CaretIndex
    {
      get { return (int)_caretIndexProperty.GetValue(); }
      set { _caretIndexProperty.SetValue(value); }
    }

    public Property FontProperty
    {
      get { return _fontProperty; }
    }

    public string Font
    {
      get { return _fontProperty.GetValue() as string; }
      set { _fontProperty.SetValue(value); }
    }

    public Property TextWrappingProperty
    {
      get { return _textWrappingProperty; }
    }

    public string TextWrapping
    {
      get { return _textWrappingProperty.GetValue() as string; }
      set { _textWrappingProperty.SetValue(value); }
    }

    public Property TextProperty
    {
      get { return _textProperty; }
    }

    public string Text
    {
      get { return _textProperty.GetValue() as string; }
      set { _textProperty.SetValue(value); }
    }

    public Property ColorProperty
    {
      get { return _colorProperty; }
    }

    public Color Color
    {
      get { return (Color) _colorProperty.GetValue(); }
      set { _colorProperty.SetValue(value); }
    }

    void AllocFont()
    {
      if (_asset == null)
      {
        Font font = FontManager.GetScript(Font);
        if (font != null)
        {
          _asset = ContentManager.GetFont(font);
        }
      }

      if (_renderer == null)
        _renderer = new FontRender(_asset.Font);
    }

    public override void Measure(System.Drawing.SizeF availableSize)
    {
      System.Drawing.SizeF size = new System.Drawing.SizeF(32, 32);

      //Trace.WriteLine(String.Format("TextBox.Measure :{0} {1}x{2}", this.Name, (int)availableSize.Width, (int)availableSize.Height));
      
      InitializeTriggers();
      AllocFont();

      if (_asset != null)
      {

        float h = _asset.Font.LineHeight;
        size = new SizeF((float)availableSize.Width, (float)(h));
        if (availableSize.Width == 0)
          size.Width = _asset.Font.Width(Text.ToString());

      }
      float marginWidth = (float)((Margin.Left + Margin.Right) * SkinContext.Zoom.Width);
      float marginHeight = (float)((Margin.Top + Margin.Bottom) * SkinContext.Zoom.Height);
      _desiredSize = new System.Drawing.SizeF((float)Width * SkinContext.Zoom.Width, (float)Height * SkinContext.Zoom.Height);
      if (Width <= 0)
        _desiredSize.Width = (float)(size.Width - marginWidth);
      if (Height <= 0)
        _desiredSize.Height = (float)(size.Height - marginHeight);

      if (LayoutTransform != null)
      {
        ExtendedMatrix m = new ExtendedMatrix();
        LayoutTransform.GetTransform(out m);
        SkinContext.AddLayoutTransform(m);
      }
      SkinContext.FinalLayoutTransform.TransformSize(ref _desiredSize);

      if (LayoutTransform != null)
      {
        SkinContext.RemoveLayoutTransform();
      }
      _desiredSize.Width += marginWidth;
      _desiredSize.Height += marginHeight;

      _availableSize = new SizeF(availableSize.Width, availableSize.Height);
    }

    public override void Arrange(System.Drawing.RectangleF finalRect)
    {
      AllocFont();
      _finalRect = new System.Drawing.RectangleF(finalRect.Location, finalRect.Size);
      System.Drawing.RectangleF layoutRect = new System.Drawing.RectangleF(finalRect.X, finalRect.Y, finalRect.Width, finalRect.Height);

      layoutRect.X += (float)(Margin.Left * SkinContext.Zoom.Width);
      layoutRect.Y += (float)(Margin.Top * SkinContext.Zoom.Height);
      layoutRect.Width -= (float)((Margin.Left + Margin.Right) * SkinContext.Zoom.Width);
      layoutRect.Height -= (float)((Margin.Top + Margin.Bottom) * SkinContext.Zoom.Height);
      ActualPosition = new Vector3(layoutRect.Location.X, layoutRect.Location.Y, 1.0f); ;
      ActualWidth = layoutRect.Width;
      ActualHeight = layoutRect.Height;
      if (LayoutTransform != null)
      {
        ExtendedMatrix m = new ExtendedMatrix();
        LayoutTransform.GetTransform(out m);
        SkinContext.AddLayoutTransform(m);
      }
      if (LayoutTransform != null)
      {
        SkinContext.RemoveLayoutTransform();
      }
      _finalLayoutTransform = SkinContext.FinalLayoutTransform;
      IsArrangeValid = true;
      _isLayoutInvalid = false;
      _update = true;
      //Trace.WriteLine(String.Format("TextBox.arrange :{0} {1},{2} {3}x{4}", this.Name, (int)finalRect.X, (int)finalRect.Y, (int)finalRect.Width, (int)finalRect.Height));
    
      if (Window != null) 
        Window.Invalidate(this);
    }

    public override void DoBuildRenderTree()
    {
      if (!IsVisible) return;
      if (_asset == null) return;
      AllocFont();
      ColorValue color = ColorConverter.FromColor(this.Color);

      base.DoRender();
      //GraphicsDevice.TransformWorld = SkinContext.FinalMatrix.Matrix;
      float totalWidth;
      float size = _asset.Font.Size;
      float x = (float)ActualPosition.X;
      float y = (float)ActualPosition.Y;
      float w = (float)ActualWidth;
      float h = (float)ActualHeight;
      if (_finalLayoutTransform != null)
      {
        GraphicsDevice.TransformWorld *= _finalLayoutTransform.Matrix;

        _finalLayoutTransform.InvertXY(ref x, ref y);
        _finalLayoutTransform.InvertXY(ref w, ref h);
      }
      System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)x, (int)y, (int)w, (int)h);
      SkinEngine.Fonts.Font.Align align = SkinEngine.Fonts.Font.Align.Left;
      if (HorizontalAlignment == HorizontalAlignmentEnum.Right)
        align = SkinEngine.Fonts.Font.Align.Right;
      else if (HorizontalAlignment == HorizontalAlignmentEnum.Center)
        align = SkinEngine.Fonts.Font.Align.Center;

      if (rect.Height < _asset.Font.LineHeight * 1.2f * SkinContext.Zoom.Height)
      {
        rect.Height = (int)(_asset.Font.LineHeight * 1.2f * SkinContext.Zoom.Height);
      }
      if (VerticalAlignment == VerticalAlignmentEnum.Center)
      {
        rect.Y = (int)(y + (h - _asset.Font.LineHeight * SkinContext.Zoom.Height) / 2.0);
      }

      rect.Width = (int)(((float)rect.Width) / SkinContext.Zoom.Width);
      rect.Height = (int)(((float)rect.Height) / SkinContext.Zoom.Height);
      ExtendedMatrix m = new ExtendedMatrix();
      m.Matrix = Matrix.Translation((float)-rect.X, (float)-rect.Y, 0);
      m.Matrix *= Matrix.Scaling(SkinContext.Zoom.Width, SkinContext.Zoom.Height, 1);
      m.Matrix *= Matrix.Translation((float)rect.X, (float)rect.Y, 0);
      SkinContext.AddTransform(m);
      color.Alpha *= (float)SkinContext.Opacity;
      color.Alpha *= (float)this.Opacity;

      _renderer.Draw(Text, rect, align, size, color, false, out totalWidth);
      SkinContext.RemoveTransform();

    }

    public override void DestroyRenderTree()
    {
      if (_renderer != null)
        _renderer.Free();
      _renderer = null;
    }

    public override void DoRender()
    {
      if (SkinContext.UseBatching == false)
      {
        if (_asset == null) 
          return;
        ColorValue color = ColorConverter.FromColor(this.Color);

        base.DoRender();
        float totalWidth;
        float size = _asset.Font.Size;
        float x = (float)ActualPosition.X;
        float y = (float)ActualPosition.Y;
        float w = (float)ActualWidth;
        float h = (float)ActualHeight;
        if (_finalLayoutTransform != null)
        {
          GraphicsDevice.TransformWorld *= _finalLayoutTransform.Matrix;

          _finalLayoutTransform.InvertXY(ref x, ref y);
          _finalLayoutTransform.InvertXY(ref w, ref h);
        }
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)x, (int)y, (int)w, (int)h);
        SkinEngine.Fonts.Font.Align align = SkinEngine.Fonts.Font.Align.Left;
        if (HorizontalAlignment == HorizontalAlignmentEnum.Right)
          align = SkinEngine.Fonts.Font.Align.Right;
        else if (HorizontalAlignment == HorizontalAlignmentEnum.Center)
          align = SkinEngine.Fonts.Font.Align.Center;

        if (rect.Height < _asset.Font.LineHeight * 1.2f * SkinContext.Zoom.Height)
        {
          rect.Height = (int)(_asset.Font.LineHeight * 1.2f * SkinContext.Zoom.Height);
        }
        if (VerticalAlignment == VerticalAlignmentEnum.Center)
        {
          rect.Y = (int)(y + (h - _asset.Font.LineHeight * SkinContext.Zoom.Height) / 2.0);
        }

        rect.Width = (int)(((float)rect.Width) / SkinContext.Zoom.Width);
        rect.Height = (int)(((float)rect.Height) / SkinContext.Zoom.Height);
        ExtendedMatrix m = new ExtendedMatrix();
        m.Matrix = Matrix.Translation((float)-rect.X, (float)-rect.Y, 0);
        m.Matrix *= Matrix.Scaling(SkinContext.Zoom.Width, SkinContext.Zoom.Height, 1);
        m.Matrix *= Matrix.Translation((float)rect.X, (float)rect.Y, 0);
        SkinContext.AddTransform(m);
        color.Alpha *= (float)SkinContext.Opacity;
        color.Alpha *= (float)this.Opacity;
        _asset.Draw(Text, rect, align, size, color, false, out totalWidth);
        SkinContext.RemoveTransform();
      }
      else
      {
        if (_asset == null) return;
        ColorValue color = ColorConverter.FromColor(this.Color);

        base.DoRender();
        float totalWidth;
        float size = _asset.Font.Size;
        float x = (float)ActualPosition.X;
        float y = (float)ActualPosition.Y;
        float w = (float)ActualWidth;
        float h = (float)ActualHeight;
        if (_finalLayoutTransform != null)
        {
          GraphicsDevice.TransformWorld *= _finalLayoutTransform.Matrix;

          _finalLayoutTransform.InvertXY(ref x, ref y);
          _finalLayoutTransform.InvertXY(ref w, ref h);
        }
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)x, (int)y, (int)w, (int)h);
        SkinEngine.Fonts.Font.Align align = SkinEngine.Fonts.Font.Align.Left;
        if (HorizontalAlignment == HorizontalAlignmentEnum.Right)
          align = SkinEngine.Fonts.Font.Align.Right;
        else if (HorizontalAlignment == HorizontalAlignmentEnum.Center)
          align = SkinEngine.Fonts.Font.Align.Center;

        if (rect.Height < _asset.Font.LineHeight * 1.2f * SkinContext.Zoom.Height)
        {
          rect.Height = (int)(_asset.Font.LineHeight * 1.2f * SkinContext.Zoom.Height);
        }
        if (VerticalAlignment == VerticalAlignmentEnum.Center)
        {
          rect.Y = (int)(y + (h - _asset.Font.LineHeight * SkinContext.Zoom.Height) / 2.0);
        }

        rect.Width = (int)(((float)rect.Width) / SkinContext.Zoom.Width);
        rect.Height = (int)(((float)rect.Height) / SkinContext.Zoom.Height);
        ExtendedMatrix m = new ExtendedMatrix();
        m.Matrix = Matrix.Translation((float)-rect.X, (float)-rect.Y, 0);
        m.Matrix *= Matrix.Scaling(SkinContext.Zoom.Width, SkinContext.Zoom.Height, 1);
        m.Matrix *= Matrix.Translation((float)rect.X, (float)rect.Y, 0);
        SkinContext.AddTransform(m);
        color.Alpha *= (float)SkinContext.Opacity;
        color.Alpha *= (float)this.Opacity;
        _renderer.Draw(Text, rect, align, size, color, false, out totalWidth);
        SkinContext.RemoveTransform();
      }
    }

    public override void Deallocate()
    {
      base.Deallocate();
      if (_asset != null)
      {
        ContentManager.Remove(_asset);
        _asset.Free(true);
        _asset = null;
      }
      if (_renderer != null)
        _renderer.Free();
      _renderer = null;
    }

    public override void BecomesHidden()
    {
      if (_renderer != null)
        _renderer.Free();
    }

    public override void BecomesVisible()
    {

      if (_renderer != null)
      {
        _renderer.Alloc();
        DoBuildRenderTree();
      }
    }
    
    public override void FireUIEvent(UIEvent eventType, UIElement source)
    {
      base.FireUIEvent(eventType, source);
    }
    
    public override void Update()
    {
      base.Update();
      if (_hidden == false)
      {
        if (_update && _renderer != null)
        {
          DoBuildRenderTree();
        }
      }
      _update = false;
    }

    public override void OnKeyPressed(ref Key key)
    {
      Boolean predict = true;
      if (!HasFocus) 
        return;
      if (key == MediaPortal.Control.InputManager.Key.None)
        return;
     
      _editText = true;

      if (key == MediaPortal.Control.InputManager.Key.BackSpace)
      {
        if (CaretIndex > 0)
        {
          Text = Text.Remove(CaretIndex - 1, 1);
          CaretIndex = CaretIndex - 1;
        }
      }
      else if (key == MediaPortal.Control.InputManager.Key.Left)
      {
        if (CaretIndex > 0)
        {
          CaretIndex = CaretIndex - 1;
          predict = false;
        }
      }
      else if (key == MediaPortal.Control.InputManager.Key.Right)
      {
        if (CaretIndex < Text.Length)
        {
          CaretIndex = CaretIndex + 1;
          predict = false;
        }
        
      }
      else if (key == MediaPortal.Control.InputManager.Key.Home)
      {
        CaretIndex = 0;
      }
      else if (key == MediaPortal.Control.InputManager.Key.End)
      {
        CaretIndex = Text.Length;
      } 
      else if (key != MediaPortal.Control.InputManager.Key.Up && 
               key != MediaPortal.Control.InputManager.Key.Down &&
               key != MediaPortal.Control.InputManager.Key.Enter)
      {
        Text = Text.Insert(CaretIndex, key.Name);
        CaretIndex = CaretIndex + 1;
      }

      _editText = false;

      if (predict)
      {
        UIElement cntl = FocusManager.PredictFocus(this, ref key);
        if (cntl != null)
        {
          cntl.HasFocus = true;
          key = MediaPortal.Control.InputManager.Key.None;
        }
      }
    }
  }
}

