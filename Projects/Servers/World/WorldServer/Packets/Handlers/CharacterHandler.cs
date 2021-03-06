﻿// Copyright (c) Multi-Emu.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Framework.Attributes;
using Framework.Constants.Account;
using Framework.Constants.Net;
using Framework.Database;
using Framework.Database.Character.Entities;
using Framework.Logging;
using World.Shared.Game.Entities;
using WorldServer.Managers;
using WorldServer.Network;
using WorldServer.Packets.Client.Character;
using WorldServer.Packets.Server.Misc;
using WorldServer.Packets.Server.Spell;

namespace WorldServer.Packets.Handlers
{
    class CharacterHandler
    {
        [GlobalMessage(GlobalClientMessage.PlayerLogin, SessionState.Authenticated)]
        public static async void HandlePlayerLogin(PlayerLogin playerLogin, WorldSession session)
        {
            Log.Debug($"Character with GUID '{playerLogin.PlayerGUID.CreationBits}' tried to login...");

            var character = DB.Character.Single<Character>(c => c.Guid == playerLogin.PlayerGUID.CreationBits && c.GameAccountId == session.GameAccount.Id);

            if (character != null)
            {
                var worldNode = Manager.Redirect.GetWorldNode((int)character.Map);

                if (worldNode != null)
                {
                    // Create new player.
                    session.Player = new Player(character);

                    // Suspend the current connection & redirect
                    // Disable (causes disconnect).
                    //await session.Send(new SuspendComms());
                    await NetHandler.SendConnectTo(session, worldNode.Address, worldNode.Port, 1);

                    // Enable key bindings, etc.
                    await session.Send(new AccountDataTimes { PlayerGuid = session.Player.Guid });

                    // Send known spells
                    await session.Send(new InitialKnownSpells
                    {
                        InitialLogin = character.FirstLogin == 1,
                        KnownSpells  = character.CharacterSpells
                    });

                    // Enter world.
                    Manager.Player.EnterWorld(session);
                }
            }
            else
                session.Dispose();
        }
    }
}
