using Godot;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban
{
    public partial class OnClick : Node2D
    {

        [Export] private GpuParticles2D particles;
        private static PackedScene packedClick = (PackedScene)ResourceLoader.Load("res://Scenes/Juiciness/OnClick.tscn");


        public static OnClick Create(Vector2 pCrossPosition, Node pParent)
        {
            OnClick lClick = (OnClick)packedClick.Instantiate();
            lClick.GlobalPosition = (pCrossPosition) * Map.DISTANCE_RANGE;

            pParent.AddChild(lClick);
            return lClick;
        }

        public override void _Ready()
        {
            ZIndex = 0;
            particles.Emitting = true;
        }

        public override void _Process(double pDelta)
        {
            if (!particles.Emitting || GlobalPosition == Player.GetInstance().GlobalPosition) { QueueFree(); }
        }
    }
}