namespace WhatToEat.Helpers
{
  public static class DataTypeHelper
  {
    public static int ToInt(this bool value) => value ? 1 : 0;
  }
}
