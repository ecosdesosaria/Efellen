using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Custom.KoperPets
{
    public static class PetTamingSkillGain
    {
        private static Dictionary<PlayerMobile, DateTime> _tamingCooldowns = new Dictionary<PlayerMobile, DateTime>();
        private static Dictionary<PlayerMobile, DateTime> _markGainCooldowns = new Dictionary<PlayerMobile, DateTime>();

        private static readonly TimeSpan TamingCooldown = TimeSpan.FromSeconds(MyServerSettings.KoperCooldown());
        private static readonly TimeSpan MarkGainCooldown = TimeSpan.FromSeconds(120);
        
        private static readonly string[] TamingMessages = new string[]
        {
            "*{0} parece confiar mais em seu mestre.*",
            "*{0} aparenta estar mais em sintonia com seu mestre.*",
            "*{0} move-se com maior confiança ao lado de seu mestre.*",
            "*{0} olha para seu mestre com entendimento recém-descoberto.*",
            "*{0} segue a orientação de seu mestre mais de perto.*",
            "*{0} parece ficar mais forte sob a liderança de seu mestre.*",
            "*{0} e seu mestre lutam em perfeita harmonia.*",
            "*{0} move-se com maior coordenação ao lado de seu mestre.*",
            "*{0} observa seu mestre cuidadosamente, aprendendo com cada movimento.*",
            "*{0} responde aos comandos com precisão aumentada.*",
            "*{0} luta como se verdadeiramente ligado a seu mestre.*",
            "*{0} aparenta ser mais dedicado à causa de seu mestre.*",
            "*{0} segue seu mestre com lealdade inabalável.*",
            "*{0} porta-se com um recém-descoberto senso de propósito.*",
            "*{0} parece mais confiante com seu mestre ao lado.*",
            "*{0} aparenta estar mais sintonizado com a presença de seu mestre.*",
            "*{0} confia mais profundamente em seu mestre após a batalha.*",
            "*{0} tornou-se mais afinado com os instintos de seu mestre.*",
            "*{0} aprende com os movimentos de seu mestre, adaptando-se rapidamente.*",
            "*{0} luta com a precisão de um companheiro bem treinado.*",
            "*{0} e seu mestre movem-se como um só em batalha.*",
            "*{0} parece ter fortalecido seu vínculo através do combate.*",
            "*{0} observa seu mestre com admiração e compreensão.*",
            "*{0} parece sentir uma conexão mais forte com seu mestre.*",
            "*{0} reage instantaneamente aos comandos não falados de seu mestre.*",
            "*{0} permanece ao lado de seu mestre com determinação renovada.*",
            "*{0} parece mais sintonizado com as emoções e intenções de seu mestre.*",
            "*{0} move-se com sincronização perfeita ao lado de seu mestre.*",
            "*{0} luta com a dedicação de um companheiro verdadeiramente leal.*",
            "*{0} porta-se como um guerreiro experiente ao lado de seu mestre.*"
        };

        private static readonly string[] BattleCries = new string[]
        {
            "*{0} luta com tudo o que tem!*",
            "*{0} defende seu mestre com lealdade inabalável!*",
            "*{0} investe contra o inimigo com força total!*",
            "*{0} se recusa a recuar!*",
            "*{0} avança destemido para a batalha!*",
            "*{0} luta com unhas e dentes para proteger seu mestre!*",
            "*{0} move-se com velocidade e precisão incríveis!*",
            "*{0} ataca ferozmente, sem mostrar misericórdia!*",
            "*{0} lança um ataque devastador!*",
            "*{0} luta como um verdadeiro guerreiro da natureza!*",
            "*{0} mantém sua posição, recusando-se a fraquejar!*",
            "*{0} debate-se com fúria bruta!*",
            "*{0} ruge em desafio enquanto ataca!*",
            "*{0} leva-se ao limite!*",
            "*{0} é um borrão de movimento, dominando seu oponente!*",
            "*{0} desencadeia uma investida poderosa!*",
            "*{0} dá tudo de si, sem se deixar abater pelo perigo!*",
            "*{0} luta com a força de uma fera desacorrentada!*",
            "*{0} é implacável, nunca cedendo um centímetro!*",
            "*{0} luta com determinação inabalável!*",
            "*{0} recusa-se a deixar seu mestre ser ferido!*",
            "*{0} luta ferozmente, guiado pelo instinto!*",
            "*{0} move-se como uma tempestade, golpeando repetidamente!*",
            "*{0} desfere um golpe esmagador!*",
            "*{0} não hesita, golpeando com força total!*",
            "*{0} dilacera seu oponente com intenção selvagem!*",
            "*{0} luta como uma besta possessa!*",
            "*{0} é implacável em sua busca pela vitória!*",
            "*{0} enfrenta seu oponente de frente, sem medo!*",
            "*{0} luta como se sua própria sobrevivência dependesse disso!*"
        };


        public static void TryTamingGain(BaseCreature pet, Mobile target)
        {
            // only apply gains to pets that are not summoned and not henchman
            if (pet == null || target == null || !MyServerSettings.KoperPets() || pet.Summoned || pet.Body.IsHuman)
                return;

            PlayerMobile owner = pet.ControlMaster as PlayerMobile;
            if (owner == null)
                return;

            // Check if player is on cooldown
            DateTime lastGainTime;
            if (_tamingCooldowns.TryGetValue(owner, out lastGainTime))
            {
                if (DateTime.UtcNow < lastGainTime + TamingCooldown)
                {
                    return; // Player is still on cooldown, exit without gaining skill
                }
            }

            double tamingSkill = owner.Skills[SkillName.Taming].Base;
            double gainChance = 0.0;
            double tamingMultiplier = MyServerSettings.KoperTamingChance();  // Determine gain chance and amount based on skill level

            DateTime lastMarkGainTime;
            bool canGainMarks = true;
            
            if (_markGainCooldowns.TryGetValue(owner, out lastMarkGainTime))
            {
                if (DateTime.UtcNow < lastMarkGainTime + MarkGainCooldown)
                {
                    canGainMarks = false;
                }
            }

            if (canGainMarks && owner.Skills[SkillName.Druidism].Base > Utility.RandomMinMax(20, 146) && owner.NpcGuild == NpcGuild.DruidsGuild)
            {
                double amount = (owner.Skills[SkillName.Druidism].Base + owner.Skills[SkillName.Taming].Base + owner.Skills[SkillName.Herding].Base + owner.Skills[SkillName.Veterinary].Base) / 45;
                int marks = amount > 2 ? Utility.RandomMinMax((int)amount / 2, (int)amount) : 0;
                
                if (marks > 0)
                {
                    owner.AddToBackpack(new MarksOfTheWilds(marks));
                    owner.SendMessage(string.Format("Você ganhou {0} Marcas das selvas.", marks));
                    
                    _markGainCooldowns[owner] = DateTime.UtcNow;
                }
            }
		
            // Determine gain chance and amount based on skill level
            if (tamingMultiplier <= 0) tamingMultiplier = 1.0; // Ensure valid value
            if (tamingMultiplier >= 10) tamingMultiplier = 10.0; // Ensure valid value
            if (tamingSkill <= 30.0) { gainChance = 0.20 * tamingMultiplier;}
            else if (tamingSkill <= 50.0) { gainChance = 0.15 * tamingMultiplier;}
            else if (tamingSkill <= 70.0) { gainChance = 0.10 * tamingMultiplier;}
            else if (tamingSkill <= 125.0) { gainChance = 0.05 * tamingMultiplier;}
            else return; // No gain if at max skill

            // Attempt taming skill gain
            if (Utility.RandomDouble() < gainChance)
            {
                owner.CheckSkill(SkillName.Taming, 0.0 , 125.0);
            
                // Select a random taming message
                string message = string.Format(TamingMessages[Utility.Random(TamingMessages.Length)], pet.Name);

                // Display message in system log
                owner.SendMessage(0x83A, message);

                // Set cooldown time for this player
                _tamingCooldowns[owner] = DateTime.UtcNow;

                // award totens for druids
                TryGrantFormTotemFromPets(owner);
            }
            else
            {
                // 10% chance to trigger a battle cry if no taming skill was gained
                TryPetBattleCry(pet);
            }
        }

        public static void TryPetBattleCry(BaseCreature pet)
        {
            if (pet == null || pet.ControlMaster == null)
                return;

            if (Utility.RandomDouble() < 0.10 && MyServerSettings.KoperPetsImmersive()) // 10% chance if no skill gain
            {
                string battleCry = String.Format(BattleCries[Utility.Random(BattleCries.Length)], pet.Name);

                // Display battle cry in grey text
                pet.PublicOverheadMessage(MessageType.Emote, 0x83A, false, battleCry);
            }
        }

        private static void TryGrantFormTotemFromPets(PlayerMobile owner)
        {
            if (owner == null || owner.Backpack == null)
                return;

            HeartOfTheWilds heart = owner.FindItemOnLayer(Layer.Neck) as HeartOfTheWilds;
            if (heart == null)
                return;

            foreach (Mobile m in owner.AllFollowers)
            {
                BaseCreature pet = m as BaseCreature;
                if (pet == null || !pet.Controlled || pet.ControlMaster != owner)
                    continue;

                DruidismFormMapping mapping = DruidismFormMapping.GetMapping(pet);
                if (mapping == null)
                    continue;

                if (owner.Skills[SkillName.Druidism].Base < mapping.RequiredDruidism)
                    continue;

                if (heart.IsFormUnlocked(mapping.FormId))
                    continue;

                double baseChance = 0.05;
                double bonus = owner.Skills[SkillName.Druidism].Value / 2500.0; // 5% at 125
                double totalChance = baseChance + bonus;

                if (totalChance > 0.10)
                    totalChance = 0.10;

                if (Utility.RandomDouble() > totalChance)
                    continue;

                if (!owner.Backpack.TryDropItem(owner, new TotemOfTheWilds(mapping.FormId), false))
                    return;

                owner.SendMessage(
                    0x59,
                    "Seu vínculo com seu companheiro revela o espírito primal do {0}.",mapping.FormId
                );

                owner.PlaySound(0x1F7);
                owner.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                return;
            }
        }

    }
}