using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class LevelData : Resource
{
    public List<List<char>> grid;
    //public List<Bomb>;
    public List<Vector2I> targetPosition;



}
