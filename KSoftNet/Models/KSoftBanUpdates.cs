﻿using System.Collections.Generic;

namespace KSoftNet.Models {
    public class KSoftBanUpdatesDatum {
        public object Id { get; set; }
        public string Reason { get; set; }
        public string Proof { get; set; }
        public object ModeratorId { get; set; }
        public bool Active { get; set; }
    }

    public class KSoftBanUpdates {
        public IList<KSoftBanUpdatesDatum> Data { get; set; }
        public int CurrentTimestamp { get; set; }
    }
}