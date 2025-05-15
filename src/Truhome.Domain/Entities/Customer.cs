using System;
using System.Collections.Generic;

namespace Truhome.Domain.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Middlename { get; set; }

    public string? Lastname { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public long? Mobilenumber { get; set; }

    public string? Drivinglicensenumber { get; set; }

    public string? Passportnumber { get; set; }

    public string? Pannumber { get; set; }

    public string? Aadharnumber { get; set; }

    public string? Ckycnumber { get; set; }

    public string? Voterid { get; set; }

    public string? Fatherfirstname { get; set; }

    public string? Fathermiddlename { get; set; }

    public string? Fatherlastname { get; set; }

    public string? Husbandfirstname { get; set; }

    public string? Husbandmiddlename { get; set; }

    public string? Husbandlastname { get; set; }
}
