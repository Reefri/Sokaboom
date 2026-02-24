using Godot;
using System;

public partial class Caisse : Node
{
	[Export] private AnimationPlayer anim;
	[Export] private GpuParticles2D moveDust;

	private const string GO_UP_ANIM = "goUp";
	private const string GO_DOWN_ANIM = "goDown";
	private const string GO_RIGHT_ANIM = "goRight";
	private const string GO_LEFT_ANIM = "goLeft";
	public override void _Ready()
	{

	}


	public override void _Process(double delta)
	{

		if (Input.IsKeyPressed(Key.Z) && !anim.IsPlaying())
		{
			anim.Play(GO_UP_ANIM);
			moveDust.Emitting = true;
		}

		if (Input.IsKeyPressed(Key.S) && !anim.IsPlaying())
		{
			anim.Play(GO_DOWN_ANIM);
            moveDust.Emitting = true;
		}

		if (Input.IsKeyPressed(Key.Q) && !anim.IsPlaying())
		{
			anim.Play(GO_LEFT_ANIM);
            moveDust.Emitting = true;
		}

		if (Input.IsKeyPressed(Key.D) && !anim.IsPlaying())
		{
			anim.Play(GO_RIGHT_ANIM);
            moveDust.Emitting = true;
		}

	}
}
