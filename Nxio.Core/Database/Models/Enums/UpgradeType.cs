using System.ComponentModel.DataAnnotations;

namespace Nxio.Core.Database.Models.Enums;

public enum UpgradeType
{
    [Display(Name = "Hit%")] PercentageIncrease = 100,
    [Display(Name = "Resist%")] PercentageResistance = 200
}