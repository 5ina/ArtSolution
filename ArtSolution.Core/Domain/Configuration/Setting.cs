using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Configuration
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Setting :Entity
    {
        public Setting() { }

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }
        

        public override string ToString()
        {
            return Name;
        }
    }
}
