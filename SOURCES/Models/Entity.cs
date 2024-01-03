﻿namespace SOURCES.Models;

public class Entity
{
    public required string Name { get; set; }
    public string? Path { get; set; } = null;
    public bool Configure { get; set; } = false;
}