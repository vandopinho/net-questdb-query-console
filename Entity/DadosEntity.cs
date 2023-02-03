namespace QuestDbQueryConsole.Entity
{
    using Microsoft.VisualBasic;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Drawing.Drawing2D;

    public partial class DadosEntity
    {
        public string Datetime { get; set; }
        public DateTime PeriodStart { get; set; }
        public string Name { get; set; }
        public float Flow { get; set; }
        public float FlowSetpoint { get; set; }
        public float Pressure { get; set; }
        public float PressureSetpoint { get; set; }
        public float OverloadValue { get; set; }
        public int OperationStatus { get; set; }
        public int OperationType { get; set; }
        public int OperationMode { get; set; }
        public override string ToString()
        {
            return string.Format("Datetime: {0}, PeriodStart: {1}, Name: {2}, Flow: {3}, FlowSetPoint: : {4}, Pressure : {5}, PressureSetPoint : {6}, OverloadValue : {7}, OperationStatus : {8}, OperationType: {9}, OperationMode : {10}",
                Datetime, PeriodStart, Name, Flow, FlowSetpoint, Pressure, PressureSetpoint, OverloadValue, OperationStatus, OperationType, OperationMode);
        }
    }

    public partial class DadosEntityWireProtocol
    {
        public string Datetime { get; set; }
        public DateTime PeriodStart { get; set; }
        public string Name { get; set; }
        public double Flow { get; set; }
        public double FlowSetpoint { get; set; }
        public double Pressure{ get; set; }
        public double PressureSetpoint { get; set; }
        public double OverloadValue { get; set; }        
        public int OperationStatus { get; set; }
        public int OperationType { get; set; }
        public int OperationMode { get; set; }
        public override string ToString()
        {
            return string.Format("Datetime: {0}, PeriodStart: {1}, Name: {2}, Flow: {3}, FlowSetPoint: : {4}, Pressure : {5}, PressureSetPoint : {6}, OverloadValue : {7}, OperationStatus : {8}, OperationType: {9}, OperationMode : {10}",
                Datetime,PeriodStart, Name ,Flow ,FlowSetpoint,Pressure, PressureSetpoint, OverloadValue, OperationStatus,OperationType,OperationMode);
        }
    }
}
