using UnityEngine.InputSystem;

public class InputManagerSingleton
{
	private static InputManagerSingleton instance;

	public InputManager Actions { get; private set; }

	private InputManagerSingleton()
	{
		Actions = new InputManager();
	}

	public static InputManagerSingleton Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new InputManagerSingleton();
			}
			return instance;
		}
	}
}
