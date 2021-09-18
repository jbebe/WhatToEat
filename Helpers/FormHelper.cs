using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatToEat.Helpers
{
  public class FormHelper
  {
    public static Func<string, IEnumerable<string>> ValidateText(int min = 0, int max = int.MaxValue)
    {
      IEnumerable<string> validate(string input)
      {
        if (input?.Length < min) yield return "Password is too short!";
        if (input?.Length > max) yield return "Password is too long!";
      }

      return validate;
    }

    public static Func<string, IEnumerable<string>> ValidateCheckboxes(Func<IEnumerable<bool>> getCheckboxes)
    {
      IEnumerable<string> validate(string input)
      {
        if (getCheckboxes().All(x => x == false)) yield return "At least one checkbox must be selected!";
      }

      return validate;
    }
  }
}
