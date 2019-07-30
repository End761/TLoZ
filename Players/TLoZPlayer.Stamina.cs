using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TLoZ.Players
{
    public sealed partial class TLoZPlayer : ModPlayer
    {
        public double maxStamina, bonusStamina;

        private double _stamina;

        private double _staminaReplenishRate;
        public double spendRate;

        public bool haltStaminaRegen, exhausted;

        private int _exhaustedTimer, _lastChangeTimer;

        private bool 
            _lastRightMovementState, _lastLeftMovementState, _sprinting;

        private int _sprintDirection;
        public void ResetStaminaEffects()
        {
            if (_exhaustedTimer > 0)
                _exhaustedTimer--;

            if (_lastChangeTimer > 0)
                _lastChangeTimer--;

            if (((player.controlRight && !_lastRightMovementState && _lastChangeTimer > 0) || (player.controlLeft && !_lastLeftMovementState && _lastChangeTimer > 0)) && !exhausted)
            {
                _sprintDirection = player.direction;
                _sprinting = true;
            }

            if ((player.controlLeft && !_lastLeftMovementState) || (player.controlRight && !_lastRightMovementState))
            {
                _lastChangeTimer = 15;
            }
            if ((!player.controlLeft && !player.controlRight) || player.direction != _sprintDirection || exhausted)
                _sprinting = false;

            if (_sprinting)
            {
                _exhaustedTimer = 30;
                spendRate += 0.12;
            }

            if (usesParaglider)
            {
                _sprinting = false;
                if (player.velocity.Y > 0)
                    spendRate += 0.12;
                if(player.velocity.Y != 0)
                    haltStaminaRegen = true;
            }

            if (spendRate > 0.0)
            {
                if (BonusStamina <= 0.0)
                    Stamina -= spendRate;
                else
                    BonusStamina -= spendRate;
            }
            else if (!haltStaminaRegen && _exhaustedTimer <= 0)
            {
                if (!exhausted)
                    Stamina += _staminaReplenishRate;
                else
                    Stamina += _staminaReplenishRate * 0.5;
            }

            UIManager.StaminaUI.rate = spendRate;
            UIManager.StaminaUI.haltRegen = haltStaminaRegen;


            if (Stamina <= 0.0 && !exhausted)
            {
                _sprinting = false;
                exhausted = true;
                _exhaustedTimer = 60;
                usesParaglider = false;
            }
            if(Stamina >= maxStamina)
            {
                exhausted = false;
            }

            _lastLeftMovementState = player.controlLeft;
            _lastRightMovementState = player.controlRight;


            spendRate = 0.0f;
            haltStaminaRegen = false;
        }
        private void InitializeStamina()
        {
            _staminaReplenishRate = 0.2;
            spendRate = 0.0;
            maxStamina = 50.0;
            _stamina = maxStamina;
        }
        public void UpdateStaminaRunSpeeds()
        {
            if (_sprinting)
            {
                player.moveSpeed *= 1.75f;
                player.maxRunSpeed *= 1.75f;
                player.runAcceleration *= 1.75f;
            }
            if(exhausted)
            {
                player.moveSpeed *= 0.5f;
                player.maxRunSpeed *= 0.5f;
                player.runAcceleration *= 0.5f;
            }
        }
        private void SaveStamina(TagCompound tag)
        {
            tag.Add("maxStamina", maxStamina);
        }
        private void LoadStamina(TagCompound tag)
        {
            if(tag.GetDouble("maxStamina") != 0)
                maxStamina = tag.GetDouble("maxStamina");
        }
        public double BonusStamina
        {
            get { return bonusStamina; }
            set
            {
                bonusStamina = value;
                if (bonusStamina < 0.0)
                    bonusStamina = 0.0;
                if (bonusStamina > maxStamina * 2)
                    bonusStamina = maxStamina * 2;
            }
        }
        public double Stamina
        {
            get { return _stamina; }
            set
            {
                _stamina = value;
                if (_stamina > maxStamina)
                    _stamina = maxStamina;
                else if (_stamina < 0.0)
                    _stamina = 0.0;
            }
        }
    }
}
