using System;

namespace Common.Models
{
    public class Birthday
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthDateString { get; set; }
        public string PhotoUrl { get; set; }
        public int Order { get; set; }
        
        // /// <summary>
        // /// Birthday is soon <= 3 days.
        // /// </summary>
        // public bool IsSoon { get; set; }
        //
        // /// <summary>
        // /// Birthday already happened.
        // /// </summary>
        // public bool IsOutDated { get; set; }
    }
}