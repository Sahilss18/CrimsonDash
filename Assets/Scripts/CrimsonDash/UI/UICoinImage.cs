using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CrimsonDash.Collectables;

namespace CrimsonDash.UI
{

	public class UICoinImage : Image
	{

		[SerializeField]
		protected ParticleSystem m_ParticleSystem;

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected override void Start()
		{
			var gm = GameManager.Singleton ?? FindFirstObjectByType<GameManager>();
			if (gm == null)
			{
				Debug.LogError("UICoinImage: GameManager not found in scene; coin UI will be disabled.");
				enabled = false;
				return;
			}

			gm.m_Coin.AddEventAndFire(Coin_OnCoinCollected, this);
		}

		void Coin_OnCoinCollected (int coinValue)
		{
			var animator = GetComponent<Animator> ();
			if (animator != null)
			{
				animator.SetTrigger ("Collect");
			}
		}

		public virtual void PlayParticleSystem ()
		{
			m_ParticleSystem.Play ();
		}
	}
}
