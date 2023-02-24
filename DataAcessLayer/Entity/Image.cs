using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity;

public class Image
{
    [Key]
    public int id { get; set; }

    public string name { get; set; }

    public string url { get; set; }
    public int ProductId { get; set; }


    #region navigation
    [ForeignKey(nameof(ProductId))]
    [JsonIgnore]
    public Products? Products { get; set; }
    #endregion
}
