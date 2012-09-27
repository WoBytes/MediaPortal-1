//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace Mediaportal.TV.Server.TVDatabase.Entities
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Channel))]
    [KnownType(typeof(Schedule))]
    [KnownType(typeof(RecordingCredit))]
    [KnownType(typeof(ProgramCategory))]
    public partial class Recording: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    
        [DataMember]
        public int idRecording
        {
            get { return _idRecording; }
            set
            {
                if (_idRecording != value)
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
                    {
                        throw new InvalidOperationException("The property 'idRecording' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                    _idRecording = value;
                    OnPropertyChanged("idRecording");
                }
            }
        }
        private int _idRecording;
    
        [DataMember]
        public Nullable<int> idChannel
        {
            get { return _idChannel; }
            set
            {
                if (_idChannel != value)
                {
                    ChangeTracker.RecordOriginalValue("idChannel", _idChannel);
                    if (!IsDeserializing)
                    {
                        if (Channel != null && Channel.IdChannel != value)
                        {
                            Channel = null;
                        }
                    }
                    _idChannel = value;
                    OnPropertyChanged("idChannel");
                }
            }
        }
        private Nullable<int> _idChannel;
    
        [DataMember]
        public System.DateTime startTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged("startTime");
                }
            }
        }
        private System.DateTime _startTime;
    
        [DataMember]
        public System.DateTime endTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged("endTime");
                }
            }
        }
        private System.DateTime _endTime;
    
        [DataMember]
        public string title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("title");
                }
            }
        }
        private string _title;
    
        [DataMember]
        public string description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("description");
                }
            }
        }
        private string _description;
    
        [DataMember]
        public string fileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged("fileName");
                }
            }
        }
        private string _fileName;
    
        [DataMember]
        public int keepUntil
        {
            get { return _keepUntil; }
            set
            {
                if (_keepUntil != value)
                {
                    _keepUntil = value;
                    OnPropertyChanged("keepUntil");
                }
            }
        }
        private int _keepUntil;
    
        [DataMember]
        public Nullable<System.DateTime> keepUntilDate
        {
            get { return _keepUntilDate; }
            set
            {
                if (_keepUntilDate != value)
                {
                    _keepUntilDate = value;
                    OnPropertyChanged("keepUntilDate");
                }
            }
        }
        private Nullable<System.DateTime> _keepUntilDate;
    
        [DataMember]
        public int timesWatched
        {
            get { return _timesWatched; }
            set
            {
                if (_timesWatched != value)
                {
                    _timesWatched = value;
                    OnPropertyChanged("timesWatched");
                }
            }
        }
        private int _timesWatched;
    
        [DataMember]
        public int stopTime
        {
            get { return _stopTime; }
            set
            {
                if (_stopTime != value)
                {
                    _stopTime = value;
                    OnPropertyChanged("stopTime");
                }
            }
        }
        private int _stopTime;
    
        [DataMember]
        public string episodeName
        {
            get { return _episodeName; }
            set
            {
                if (_episodeName != value)
                {
                    _episodeName = value;
                    OnPropertyChanged("episodeName");
                }
            }
        }
        private string _episodeName;
    
        [DataMember]
        public string seriesNum
        {
            get { return _seriesNum; }
            set
            {
                if (_seriesNum != value)
                {
                    _seriesNum = value;
                    OnPropertyChanged("seriesNum");
                }
            }
        }
        private string _seriesNum;
    
        [DataMember]
        public string episodeNum
        {
            get { return _episodeNum; }
            set
            {
                if (_episodeNum != value)
                {
                    _episodeNum = value;
                    OnPropertyChanged("episodeNum");
                }
            }
        }
        private string _episodeNum;
    
        [DataMember]
        public string episodePart
        {
            get { return _episodePart; }
            set
            {
                if (_episodePart != value)
                {
                    _episodePart = value;
                    OnPropertyChanged("episodePart");
                }
            }
        }
        private string _episodePart;
    
        [DataMember]
        public bool isRecording
        {
            get { return _isRecording; }
            set
            {
                if (_isRecording != value)
                {
                    _isRecording = value;
                    OnPropertyChanged("isRecording");
                }
            }
        }
        private bool _isRecording;
    
        [DataMember]
        public Nullable<int> idSchedule
        {
            get { return _idSchedule; }
            set
            {
                if (_idSchedule != value)
                {
                    ChangeTracker.RecordOriginalValue("idSchedule", _idSchedule);
                    if (!IsDeserializing)
                    {
                        if (Schedule != null && Schedule.id_Schedule != value)
                        {
                            Schedule = null;
                        }
                    }
                    _idSchedule = value;
                    OnPropertyChanged("idSchedule");
                }
            }
        }
        private Nullable<int> _idSchedule;
    
        [DataMember]
        public int mediaType
        {
            get { return _mediaType; }
            set
            {
                if (_mediaType != value)
                {
                    _mediaType = value;
                    OnPropertyChanged("mediaType");
                }
            }
        }
        private int _mediaType;
    
        [DataMember]
        public Nullable<int> idProgramCategory
        {
            get { return _idProgramCategory; }
            set
            {
                if (_idProgramCategory != value)
                {
                    ChangeTracker.RecordOriginalValue("idProgramCategory", _idProgramCategory);
                    if (!IsDeserializing)
                    {
                        if (ProgramCategory != null && ProgramCategory.idProgramCategory != value)
                        {
                            ProgramCategory = null;
                        }
                    }
                    _idProgramCategory = value;
                    OnPropertyChanged("idProgramCategory");
                }
            }
        }
        private Nullable<int> _idProgramCategory;

        #endregion
        #region Navigation Properties
    
        [DataMember]
        public Channel Channel
        {
            get { return _channel; }
            set
            {
                if (!ReferenceEquals(_channel, value))
                {
                    var previousValue = _channel;
                    _channel = value;
                    FixupChannel(previousValue);
                    OnNavigationPropertyChanged("Channel");
                }
            }
        }
        private Channel _channel;
    
        [DataMember]
        public Schedule Schedule
        {
            get { return _schedule; }
            set
            {
                if (!ReferenceEquals(_schedule, value))
                {
                    var previousValue = _schedule;
                    _schedule = value;
                    FixupSchedule(previousValue);
                    OnNavigationPropertyChanged("Schedule");
                }
            }
        }
        private Schedule _schedule;
    
        [DataMember]
        public TrackableCollection<RecordingCredit> RecordingCredits
        {
            get
            {
                if (_recordingCredits == null)
                {
                    _recordingCredits = new TrackableCollection<RecordingCredit>();
                    _recordingCredits.CollectionChanged += FixupRecordingCredits;
                }
                return _recordingCredits;
            }
            set
            {
                if (!ReferenceEquals(_recordingCredits, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
                    }
                    if (_recordingCredits != null)
                    {
                        _recordingCredits.CollectionChanged -= FixupRecordingCredits;
                        // This is the principal end in an association that performs cascade deletes.
                        // Remove the cascade delete event handler for any entities in the current collection.
                        foreach (RecordingCredit item in _recordingCredits)
                        {
                            ChangeTracker.ObjectStateChanging -= item.HandleCascadeDelete;
                        }
                    }
                    _recordingCredits = value;
                    if (_recordingCredits != null)
                    {
                        _recordingCredits.CollectionChanged += FixupRecordingCredits;
                        // This is the principal end in an association that performs cascade deletes.
                        // Add the cascade delete event handler for any entities that are already in the new collection.
                        foreach (RecordingCredit item in _recordingCredits)
                        {
                            ChangeTracker.ObjectStateChanging += item.HandleCascadeDelete;
                        }
                    }
                    OnNavigationPropertyChanged("RecordingCredits");
                }
            }
        }
        private TrackableCollection<RecordingCredit> _recordingCredits;
    
        [DataMember]
        public ProgramCategory ProgramCategory
        {
            get { return _programCategory; }
            set
            {
                if (!ReferenceEquals(_programCategory, value))
                {
                    var previousValue = _programCategory;
                    _programCategory = value;
                    FixupProgramCategory(previousValue);
                    OnNavigationPropertyChanged("ProgramCategory");
                }
            }
        }
        private ProgramCategory _programCategory;

        #endregion
        #region ChangeTracking
    
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        protected virtual void OnNavigationPropertyChanged(String propertyName)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
        private event PropertyChangedEventHandler _propertyChanged;
        private ObjectChangeTracker _changeTracker;
    
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if(_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }
    
        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }
    
        protected bool IsDeserializing { get; private set; }
    
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }
    
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
            ChangeTracker.ChangeTrackingEnabled = true;
        }
    
        protected virtual void ClearNavigationProperties()
        {
            Channel = null;
            Schedule = null;
            RecordingCredits.Clear();
            ProgramCategory = null;
        }

        #endregion
        #region Association Fixup
    
        private void FixupChannel(Channel previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Recordings.Contains(this))
            {
                previousValue.Recordings.Remove(this);
            }
    
            if (Channel != null)
            {
                if (!Channel.Recordings.Contains(this))
                {
                    Channel.Recordings.Add(this);
                }
    
                idChannel = Channel.IdChannel;
            }
            else if (!skipKeys)
            {
                idChannel = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Channel")
                    && (ChangeTracker.OriginalValues["Channel"] == Channel))
                {
                    ChangeTracker.OriginalValues.Remove("Channel");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Channel", previousValue);
                }
                if (Channel != null && !Channel.ChangeTracker.ChangeTrackingEnabled)
                {
                    Channel.StartTracking();
                }
            }
        }
    
        private void FixupSchedule(Schedule previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Recordings.Contains(this))
            {
                previousValue.Recordings.Remove(this);
            }
    
            if (Schedule != null)
            {
                if (!Schedule.Recordings.Contains(this))
                {
                    Schedule.Recordings.Add(this);
                }
    
                idSchedule = Schedule.id_Schedule;
            }
            else if (!skipKeys)
            {
                idSchedule = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("Schedule")
                    && (ChangeTracker.OriginalValues["Schedule"] == Schedule))
                {
                    ChangeTracker.OriginalValues.Remove("Schedule");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("Schedule", previousValue);
                }
                if (Schedule != null && !Schedule.ChangeTracker.ChangeTrackingEnabled)
                {
                    Schedule.StartTracking();
                }
            }
        }
    
        private void FixupProgramCategory(ProgramCategory previousValue, bool skipKeys = false)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (previousValue != null && previousValue.Recordings.Contains(this))
            {
                previousValue.Recordings.Remove(this);
            }
    
            if (ProgramCategory != null)
            {
                if (!ProgramCategory.Recordings.Contains(this))
                {
                    ProgramCategory.Recordings.Add(this);
                }
    
                idProgramCategory = ProgramCategory.idProgramCategory;
            }
            else if (!skipKeys)
            {
                idProgramCategory = null;
            }
    
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey("ProgramCategory")
                    && (ChangeTracker.OriginalValues["ProgramCategory"] == ProgramCategory))
                {
                    ChangeTracker.OriginalValues.Remove("ProgramCategory");
                }
                else
                {
                    ChangeTracker.RecordOriginalValue("ProgramCategory", previousValue);
                }
                if (ProgramCategory != null && !ProgramCategory.ChangeTracker.ChangeTrackingEnabled)
                {
                    ProgramCategory.StartTracking();
                }
            }
        }
    
        private void FixupRecordingCredits(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsDeserializing)
            {
                return;
            }
    
            if (e.NewItems != null)
            {
                foreach (RecordingCredit item in e.NewItems)
                {
                    item.Recording = this;
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties("RecordingCredits", item);
                    }
                    // This is the principal end in an association that performs cascade deletes.
                    // Update the event listener to refer to the new dependent.
                    ChangeTracker.ObjectStateChanging += item.HandleCascadeDelete;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (RecordingCredit item in e.OldItems)
                {
                    if (ReferenceEquals(item.Recording, this))
                    {
                        item.Recording = null;
                    }
                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties("RecordingCredits", item);
                    }
                    // This is the principal end in an association that performs cascade deletes.
                    // Remove the previous dependent from the event listener.
                    ChangeTracker.ObjectStateChanging -= item.HandleCascadeDelete;
                }
            }
        }

        #endregion
    }
}
