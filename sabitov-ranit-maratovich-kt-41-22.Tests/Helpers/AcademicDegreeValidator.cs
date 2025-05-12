using System.Text.RegularExpressions;
using RanitSabitovKt_41_22.Models;

namespace sabitov_ranit_maratovich_kt_41_22.Tests.Helpers.Validators
{
    public static class AcademicDegreeValidator
    {
        public static bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[А-Я][а-я]+\s[а-я]+$");
        }
    }
}