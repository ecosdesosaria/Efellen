using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Custom.KoperPets
{
    public static class KoperHerdingGain
    {
        private static readonly Dictionary<Mobile, DateTime> _cooldowns = new Dictionary<Mobile, DateTime>();
        private static readonly TimeSpan CooldownTime = TimeSpan.FromSeconds(MyServerSettings.KoperCooldown()); //  Set cooldown time (20 Seconds default)

        private static readonly string[] SuccessMessages = new string[]
        {
            "Você se sente mais confiante em guiar animais.",
            "Sua compreensão do comportamento animal melhora.",
            "Você refina sua habilidade de controlar criaturas.",
            "Seus instintos de pastoreio ficam mais fortes.",
            "Você sente uma conexão mais profunda com os animais.",
            "Você observa a linguagem corporal sutil do rebanho.",
            "As criaturas parecem responder mais facilmente aos seus comandos.",
            "Você aprende a antecipar os movimentos dos animais.",
            "Sua paciência com o rebanho compensa.",
            "Você desenvolve um ritmo ao direcionar os animais.",
            "O vínculo entre você e seus animais se fortalece.",
            "Você nota uma melhora na rapidez com que os animais obedecem.",
            "Você ganha entendimento sobre os instintos das criaturas que guia.",
            "Você domina uma nova técnica no controle de animais teimosos.",
            "Sua orientação constante faz os animais confiarem mais em você."
        };

        private static readonly string[] BondingMessages = new string[]
        {
            "Seu profundo entendimento do comportamento animal forjou um vínculo especial!",
            "Através do seu pastoreio habilidoso, a criatura passou a confiar completamente em você.",
            "Sua orientação paciente conquistou a lealdade inabalável da criatura.",
            "O animal olha para você com devoção e confiança recém-descobertas.",
            "Sua expertise em pastoreio criou um vínculo que durará a vida toda.",
            "Os olhos da criatura refletem uma conexão profunda forjada através de sua habilidade."
        };

        public static void TryGainHerdingSkill(Mobile owner)
        {

            bool hasNonSummoned = false;
            bool hasNonHumanBody = false;

            if (owner.Map != null)
            {
            	IPooledEnumerable eable = owner.Map.GetMobilesInRange(owner.Location, 5);

            	foreach (Mobile m in eable)
            	{
            		if (m is BaseCreature)
            		{
            			BaseCreature pet = (BaseCreature)m;
                        // we need to check if the player has followers that are not summons or henchman before applying the rest of the function
            			if (pet.Controlled && pet.ControlMaster == owner)
            			{
            				if (!pet.Summoned)
            					hasNonSummoned = true;

            				if (!pet.Body.IsHuman)
            					hasNonHumanBody = true;

            				if (hasNonSummoned && hasNonHumanBody)
            					break;
            			}
            		}
            	}
            	eable.Free();
            }


            if (owner == null || !owner.Alive || !MyServerSettings.KoperPets() || !hasNonSummoned || !hasNonHumanBody)
                return; // No skill gain for dead players/system disabled// or players that have only summons or henchman

            // Check if the player is on cooldown
            if (_cooldowns.ContainsKey(owner) && DateTime.UtcNow < _cooldowns[owner])
            {
                return; // Cooldown is active, exit without giving skill
            }

            double herdingSkill = owner.Skills[SkillName.Herding].Base;
            double gainChance;
            double herdingMultiplier = MyServerSettings.KoperHerdingChance();


            // Determine gain chance and amount based on skill level
            if (herdingMultiplier <= 0) herdingMultiplier = 1.0; // Ensure valid value
            if (herdingMultiplier >= 10) herdingMultiplier = 10.0; // Ensure valid value
            if (herdingSkill <= 30.0) { gainChance = 0.20 * herdingMultiplier;}
            else if (herdingSkill <= 50.0) { gainChance = 0.15 * herdingMultiplier;}
            else if (herdingSkill <= 70.0) { gainChance = 0.10 * herdingMultiplier;}
            else if (herdingSkill < 125.0) { gainChance = 0.05 * herdingMultiplier;}
            else return; // No gain if at max skill

            if (Utility.RandomDouble() <= gainChance)
            {
                owner.CheckSkill(SkillName.Herding, 0.0 , 125.0 );

                // Check for pet bonding after skill check
                TryBondUnbondedPets(owner, herdingSkill);

                // Select a random message for variety
                if (MyServerSettings.KoperPetsImmersive()) 
                {
                    owner.SendMessage(SuccessMessages[Utility.Random(SuccessMessages.Length)]);
                }
                // Start cooldown timer
                _cooldowns[owner] = DateTime.UtcNow + CooldownTime;
            }
        }

        private static void TryBondUnbondedPets(Mobile owner, double herdingSkill)
        {
            if (owner.Map == null) return;

            // 1% chance at skill 1, 25% chance at skill 125)
            double bondingChance = Math.Max(1.0, Math.Min(25.0, (herdingSkill / 125.0) * 25.0)) / 100.0;

            List<BaseCreature> unbondedPets = new List<BaseCreature>();

            IPooledEnumerable eable = owner.Map.GetMobilesInRange(owner.Location, 5);
            foreach (Mobile m in eable)
            {
                if (m is BaseCreature)
                {
                    BaseCreature pet = (BaseCreature)m;
                    if (pet.Controlled && pet.ControlMaster == owner && !pet.Summoned && 
                        !pet.Body.IsHuman && !pet.IsBonded && pet.BondingBegin != DateTime.MinValue)
                    {
                        unbondedPets.Add(pet);
                    }
                }
            }
            eable.Free();

            foreach (BaseCreature pet in unbondedPets)
            {
                if (Utility.RandomDouble() <= bondingChance)
                {
                    pet.IsBonded = true;
                    pet.BondingBegin = DateTime.MinValue; // Clear bonding timer
                    pet.Loyalty = BaseCreature.MaxLoyalty; // Set to maximum loyalty

                    if (MyServerSettings.KoperPetsImmersive())
                    {
                        owner.SendMessage(BondingMessages[Utility.Random(BondingMessages.Length)]);
                        owner.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "Seu " + pet.Name + " se vinculou a você!");
                    }
                }
            }
        }
    }
}