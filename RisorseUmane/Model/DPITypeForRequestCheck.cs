using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RisorseUmane.Model
{
    public class DPITypeForRequestCheck
    {
        private DpiTypeForRequest dpiTypeForRequest = null;

        public DPITypeForRequestCheck(DpiTypeForRequest dpiTypeForRequest)
        {
            this.dpiTypeForRequest = dpiTypeForRequest;
            if (dpiTypeForRequest == null) { return; }

            Id = dpiTypeForRequest.Id;
            DPITypeId = dpiTypeForRequest.DpiTypeId ?? 0;
            RequestId = dpiTypeForRequest.RequestId ?? 0;
            Description = dpiTypeForRequest.DpiType.Description;
            Size = dpiTypeForRequest.Size;
        }
        public DPITypeForRequestCheck() { }

        public int Id
        {
            get; set;
        }

        public int DPITypeId
        {
            get; set;
        }

        public int RequestId
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string Size
        {
            get; set;
        }
    }
}