using UnityEngine;

public class ProfilerMemoryLimit : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Profiling.Profiler.maxUsedMemory = 600 * 1024 * 1024;
	}
}
