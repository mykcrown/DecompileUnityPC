// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BindingsExample
{
	public class PlayerActions : PlayerActionSet
	{
		public PlayerAction Fire;

		public PlayerAction Jump;

		public PlayerAction Left;

		public PlayerAction Right;

		public PlayerAction Up;

		public PlayerAction Down;

		public PlayerTwoAxisAction Move;

		private static Func<PlayerAction, BindingSource, bool> __f__am_cache0;

		private static Action<PlayerAction, BindingSource> __f__am_cache1;

		private static Action<PlayerAction, BindingSource, BindingSourceRejectionType> __f__am_cache2;

		public PlayerActions()
		{
			this.Fire = base.CreatePlayerAction("Fire");
			this.Jump = base.CreatePlayerAction("Jump");
			this.Left = base.CreatePlayerAction("Move Left");
			this.Right = base.CreatePlayerAction("Move Right");
			this.Up = base.CreatePlayerAction("Move Up");
			this.Down = base.CreatePlayerAction("Move Down");
			this.Move = base.CreateTwoAxisPlayerAction(this.Left, this.Right, this.Down, this.Up);
		}

		public static PlayerActions CreateWithDefaultBindings()
		{
			PlayerActions playerActions = new PlayerActions();
			playerActions.Fire.AddDefaultBinding(new Key[]
			{
				Key.A
			});
			playerActions.Fire.AddDefaultBinding(InputControlType.Action1);
			playerActions.Fire.AddDefaultBinding(Mouse.LeftButton);
			playerActions.Jump.AddDefaultBinding(new Key[]
			{
				Key.Space
			});
			playerActions.Jump.AddDefaultBinding(InputControlType.Action3);
			playerActions.Jump.AddDefaultBinding(InputControlType.Back);
			playerActions.Up.AddDefaultBinding(new Key[]
			{
				Key.UpArrow
			});
			playerActions.Down.AddDefaultBinding(new Key[]
			{
				Key.DownArrow
			});
			playerActions.Left.AddDefaultBinding(new Key[]
			{
				Key.LeftArrow
			});
			playerActions.Right.AddDefaultBinding(new Key[]
			{
				Key.RightArrow
			});
			playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
			playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
			playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
			playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
			playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
			playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
			playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
			playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);
			playerActions.Up.AddDefaultBinding(Mouse.PositiveY);
			playerActions.Down.AddDefaultBinding(Mouse.NegativeY);
			playerActions.Left.AddDefaultBinding(Mouse.NegativeX);
			playerActions.Right.AddDefaultBinding(Mouse.PositiveX);
			playerActions.ListenOptions.IncludeUnknownControllers = true;
			playerActions.ListenOptions.MaxAllowedBindings = 4u;
			playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
			playerActions.ListenOptions.IncludeMouseButtons = true;
			BindingListenOptions arg_1A4_0 = playerActions.ListenOptions;
			if (PlayerActions.__f__am_cache0 == null)
			{
				PlayerActions.__f__am_cache0 = new Func<PlayerAction, BindingSource, bool>(PlayerActions._CreateWithDefaultBindings_m__0);
			}
			arg_1A4_0.OnBindingFound = PlayerActions.__f__am_cache0;
			BindingListenOptions expr_1AF = playerActions.ListenOptions;
			Delegate arg_1D2_0 = expr_1AF.OnBindingAdded;
			if (PlayerActions.__f__am_cache1 == null)
			{
				PlayerActions.__f__am_cache1 = new Action<PlayerAction, BindingSource>(PlayerActions._CreateWithDefaultBindings_m__1);
			}
			expr_1AF.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Combine(arg_1D2_0, PlayerActions.__f__am_cache1);
			BindingListenOptions expr_1E7 = playerActions.ListenOptions;
			Delegate arg_20A_0 = expr_1E7.OnBindingRejected;
			if (PlayerActions.__f__am_cache2 == null)
			{
				PlayerActions.__f__am_cache2 = new Action<PlayerAction, BindingSource, BindingSourceRejectionType>(PlayerActions._CreateWithDefaultBindings_m__2);
			}
			expr_1E7.OnBindingRejected = (Action<PlayerAction, BindingSource, BindingSourceRejectionType>)Delegate.Combine(arg_20A_0, PlayerActions.__f__am_cache2);
			return playerActions;
		}

		private static bool _CreateWithDefaultBindings_m__0(PlayerAction action, BindingSource binding)
		{
			if (binding == new KeyBindingSource(new Key[]
			{
				Key.Escape
			}))
			{
				action.StopListeningForBinding();
				return false;
			}
			return true;
		}

		private static void _CreateWithDefaultBindings_m__1(PlayerAction action, BindingSource binding)
		{
			UnityEngine.Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);
		}

		private static void _CreateWithDefaultBindings_m__2(PlayerAction action, BindingSource binding, BindingSourceRejectionType reason)
		{
			UnityEngine.Debug.Log("Binding rejected... " + reason);
		}
	}
}
