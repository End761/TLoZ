using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {
        private int _exhaustedTimer, _lastChangeTimer;

        private bool
            _lastRightMovementState, _lastLeftMovementState, _sprinting;

        public void ResetStaminaEffects()
        {
            if (_exhaustedTimer > 0)
                _exhaustedTimer--;

            if (_lastChangeTimer > 0)
                _lastChangeTimer--;

            if (((player.controlRight && !_lastRightMovementState && _lastChangeTimer > 0) || (player.controlLeft && !_lastLeftMovementState && _lastChangeTimer > 0)) && !Exhausted)
            {
                SprintDirection = player.direction;
                _sprinting = true;
            }

            if ((player.controlLeft && !_lastLeftMovementState) || (player.controlRight && !_lastRightMovementState))
            {
                _lastChangeTimer = 15;
            }
            if ((!player.controlLeft && !player.controlRight) || player.direction != SprintDirection || Exhausted)
                _sprinting = false;

            if (_sprinting)
            {
                _exhaustedTimer = 30;
                SpendRate += 0.12;
            }

            if (Paragliding)
            {
                _sprinting = false;
                if (player.velocity.Y > 0)
                    SpendRate += 0.12;
                if (player.velocity.Y != 0)
                    HaltStaminaRegen = true;
            }

            if (SpendRate > 0.0)
            {
                if (BonusStamina <= 0.0)
                    Stamina -= Stamina <= MaxStamina * 0.25f ? SpendRate * 0.5f : Stamina <= MaxStamina * 0.5f ? SpendRate * 0.75 : SpendRate;
                else
                    BonusStamina -= SpendRate;
            }
            else if (!HaltStaminaRegen && _exhaustedTimer <= 0)
            {
                if (!Exhausted)
                    Stamina += StaminaReplenishRate;
                else
                    Stamina += StaminaReplenishRate * 0.5;
            }

            if (player.whoAmI == Main.myPlayer)
            {
                UIManager.StaminaUI.Rate = SpendRate;
                UIManager.StaminaUI.HaltRegen = HaltStaminaRegen;
            }

            if (Stamina <= 0.0 && !Exhausted)
            {
                _sprinting = false;
                Exhausted = true;
                _exhaustedTimer = 60;
                Paragliding = false;
            }

            if (Stamina >= MaxStamina)
            {
                Exhausted = false;
            }

            _lastLeftMovementState = player.controlLeft;
            _lastRightMovementState = player.controlRight;


            SpendRate = 0.0f;
            HaltStaminaRegen = false;
        }

        private void InitializeStamina()
        {
            StaminaReplenishRate = 0.2;
            SpendRate = 0.0;
            MaxStamina = 50.0;
            _stamina = MaxStamina;
        }

        public void UpdateStaminaRunSpeeds()
        {
            if (_sprinting)
            {
                player.moveSpeed *= 1.75f;
                player.maxRunSpeed *= 1.75f;
                player.runAcceleration *= 1.75f;
            }

            if (Exhausted)
            {
                player.moveSpeed *= 0.5f;
                player.maxRunSpeed *= 0.5f;
                player.runAcceleration *= 0.5f;
            }
        }

        private void SaveStamina(TagCompound tag)
        {
            tag.Add("maxStamina", MaxStamina);
        }

        private void LoadStamina(TagCompound tag)
        {
            if (tag.GetDouble("maxStamina") != 0)
                MaxStamina = tag.GetDouble("maxStamina");
        }

        private double _bonusStamina;
        public double BonusStamina
        {
            get { return _bonusStamina; }
            set
            {
                _bonusStamina = value;
                if (_bonusStamina < 0.0)
                    _bonusStamina = 0.0;
                if (_bonusStamina > MaxStamina * 2)
                    _bonusStamina = MaxStamina * 2;
            }
        }

        private double _stamina;
        public double Stamina
        {
            get { return _stamina; }
            set
            {
                _stamina = value;
                if (_stamina > MaxStamina)
                    _stamina = MaxStamina;
                else if (_stamina < 0.0)
                    _stamina = 0.0;
            }
        }

        public double MaxStamina { get; set; }

        public double SpendRate { get; set; }

        public bool HaltStaminaRegen { get; set; }
        public bool Exhausted { get; private set; }

        public int SprintDirection { get; private set; }

        public double StaminaReplenishRate { get; private set; }
    }
}
