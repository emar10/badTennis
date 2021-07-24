using Godot;
using System;

public class Player : RigidBody
{
	public int Speed = 20000;
	public int Power = 5;
	
	private AnimatedSprite3D sprite;
	private RigidBody ball;
	private bool allowHit = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite3D>("Sprite");
		ball = GetParent().GetNode<RigidBody>("Ball");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (allowHit && Input.IsKeyPressed((int)Godot.KeyList.Space)) {
			allowHit = false;
			Hit();
		}

		if (!Input.IsKeyPressed((int)Godot.KeyList.Space)) {
			allowHit = true;
		}

		bool moving = false;
		string direction = "_up";

		if (Input.IsKeyPressed(((int)Godot.KeyList.Right))) {
			AddForce(new Vector3(Speed * delta, 0, 0), Vector3.Right);
			moving = true;
		}
		if (Input.IsKeyPressed(((int)Godot.KeyList.Left))) {
			AddForce(new Vector3(-Speed * delta, 0, 0), Vector3.Left);
			moving = true;
		}
		if (Input.IsKeyPressed(((int)Godot.KeyList.Up))) {
			AddForce(new Vector3(0, 0, -Speed * delta), Vector3.Forward);
			direction = "_up";
			moving = true;
		}
		if (Input.IsKeyPressed(((int)Godot.KeyList.Down))) {
			AddForce(new Vector3(0, 0, Speed * delta), Vector3.Back);
			direction = "_down";
			moving = true;
		}

		if (!moving && (sprite.Playing || sprite.Animation != "stand" + direction)) {
			sprite.Animation = "stand" + direction;
			sprite.Stop();
		}
		else if (moving && (!sprite.Playing || sprite.Animation != "run" + direction)) {
			sprite.Animation = "run" + direction;
			sprite.Play();
		}
	}

	public void Hit() {
		Vector3 ballRelativePosition = ball.Transform.origin - Transform.origin;
		if (ballRelativePosition.Length() < 10) {
			ballRelativePosition.y = 5;
			ball.ApplyCentralImpulse(ballRelativePosition * Power);
		}
	}
}
