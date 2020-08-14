﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManager.ViewObjects
{
    public class ParticipateActivityVO
    {
        public long StudentId { get; set; }
        public long ActivityId { get; set; }
        public DateTime ApplyDate { get; set; }
        public string ApplyReason { get; set; }
        public bool? Status { get; set; }
    }
}