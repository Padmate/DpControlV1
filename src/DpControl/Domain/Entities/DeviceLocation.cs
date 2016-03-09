﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class DeviceLocation
    {
        public int DeviceLocationId { get; set; }           // one-to-one relation with DeviceAlarm requires the primary key of dependent table used as the foreign key
        public string Building { get; set; }
        public string Floor { get; set; }
        public string RoomNo { get; set; }

        //enum Orientation
        public int Orientation { get; set; }
        public int InstallationNumber { get; set; }

        public int CurrentPosition { get; set; }
        public string CommAddress { get; set; }
        public string DeviceSerialNo { get; set; }
        ////enum DeviceType
        public int DeviceType { get; set; }
        //enum CommMode
        public int CommMode { get; set; }
        public int FavorPositionFirst { get; set; }
        public int FavorPositionrSecond { get; set; }
        public int FavorPositionThird { get; set; }

        public string Description { get; set; }
        #region Realtionship
        public virtual List<GroupDeviceLocation> GroupDeviceLocations { get; set; }                // many-to-many： 
        public virtual List<UserDeviceLocation> UserDeviceLocation { get; set; }
        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<Alarm> Alarms { get; set; }
        public virtual List<Log> Logs { get; set; }

        #endregion

        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public byte[] RowVersion { get; set; }

        public DeviceLocation()
        {
            this.GroupDeviceLocations = new List<GroupDeviceLocation>();
            this.UserDeviceLocation = new List<UserDeviceLocation>();
            this.Alarms = new List<Alarm>();
            this.Logs = new List<Log>();
        }
    }

    public enum Orientation
    {
        Null,       // for unidentified
        East,
        South,
        West,
        North,
        Southeast,
        Southwest,
        Northeast,
        Northwest
    }

    public enum DeviceType
    {
        Null,
        Controller,
        TouchScreen,
        DryContact,
        RF,
        Tubular
    }

    public enum CommMode
    {
        Null,
        RS485,
        ZigBee,
        Bluetooth,
        WiFi
    }
}
