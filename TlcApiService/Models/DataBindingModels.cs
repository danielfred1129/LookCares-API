using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TlcDataAccess;

namespace TlcApiService.Models
{
    public class FrameBindingModel
    {
        [Required]
        public tbFrame Frame { get; set; }

        public List<tbFabric> Fabrics { get; set; }
    }

    public class FabricBindingModel
    {
        /*
          ,[kLookClient]
          ,[kLookClientCustomer]
          ,[kFrame]
          ,[vcItemNumber]
          ,[vcSerialNumber]
          ,[intHeight]
          ,[intWidth]
          ,[vcExtrusion]
          ,[vcFileName]
          ,[vcNFCUrl]
          ,[vcLookCSR]
          ,[dtShippedFromTLC]
          ,[dtInstalled]
          ,[vcSource]
          ,[vcClientID]
          ,[vcClientName]
          ,[vcStatus]
          ,[vcFileName1]
          ,[vcType1]
          ,[intFileSize1]
          ,[fFile1]
          ,[vcFileName2]
          ,[vcType2]
          ,[intFileSize2]
          ,[fFile2]
      */

        [Required]
        public int ClientLocationKey { get; set; }

        [Required]
        public int FrameKey { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public string InStoreLocation { get; set; }

        //[Required]
        //public string FileName { get; set; }

    }
}