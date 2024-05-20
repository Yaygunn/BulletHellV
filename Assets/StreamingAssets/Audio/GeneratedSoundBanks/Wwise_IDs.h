/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID AMBTEST = 2338340215U;
        static const AkUniqueID AMBUNDERWATER = 3813711432U;
        static const AkUniqueID BULLET_BOUNCE = 2484480848U;
        static const AkUniqueID BULLET_EXPADING = 1097487594U;
        static const AkUniqueID BULLET_EXPLOSION = 2798586757U;
        static const AkUniqueID BULLET_HEALTH = 2721115098U;
        static const AkUniqueID BULLETTEST = 1248641489U;
        static const AkUniqueID ENE_MOB_DEATH = 1195401469U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID MUSIC_STOP = 3227181061U;
        static const AkUniqueID PLY_HEALTH_PAUSE = 545330280U;
        static const AkUniqueID PLY_HEALTH_PLAY = 4032423056U;
        static const AkUniqueID PLY_HEALTH_RESUME = 567373571U;
        static const AkUniqueID PLY_HEALTH_STOP = 2381008178U;
        static const AkUniqueID PLY_HURT = 3511115374U;
        static const AkUniqueID PLYDASH = 474402578U;
        static const AkUniqueID PLYMOVE = 216950137U;
        static const AkUniqueID SQUIDDEATH = 2782996605U;
        static const AkUniqueID SQUIDIDLE = 1480519155U;
        static const AkUniqueID SQUIDSHOOT = 3280154138U;
        static const AkUniqueID UICLOSE = 3030481697U;
        static const AkUniqueID UIDASHREADY = 2214371868U;
        static const AkUniqueID UINAV = 1038020662U;
        static const AkUniqueID UIOPEN = 1389761091U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace MUSIC
        {
            static const AkUniqueID GROUP = 3991942870U;

            namespace STATE
            {
                static const AkUniqueID MUSIC_GAMEPLAY = 620878633U;
                static const AkUniqueID MUSIC_PAUSED = 2993614867U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace MUSIC

        namespace MUSIC_SYSTEM
        {
            static const AkUniqueID GROUP = 792781730U;

            namespace STATE
            {
                static const AkUniqueID BOSS_01 = 320199936U;
                static const AkUniqueID BOSS_02 = 320199939U;
                static const AkUniqueID BOSS_03 = 320199938U;
                static const AkUniqueID CALM = 3753286132U;
                static const AkUniqueID COMBAT = 2764240573U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace MUSIC_SYSTEM

        namespace SFX
        {
            static const AkUniqueID GROUP = 393239870U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SFX_GAMEPLAY = 3587590657U;
                static const AkUniqueID SFX_PAUSED = 3762574363U;
            } // namespace STATE
        } // namespace SFX

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID MASTER_VOLUME = 4179668880U;
        static const AkUniqueID MUSIC_VOLUME = 1006694123U;
        static const AkUniqueID PLY_HEALTH = 963702167U;
        static const AkUniqueID SFX_VOLUME = 1564184899U;
        static const AkUniqueID WATER_INTENSITY = 1348429472U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID _2DAMB = 1387833709U;
        static const AkUniqueID _3DAMB = 2616461496U;
        static const AkUniqueID AMBMASTER = 2786823217U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID NPCMASTER = 2033911932U;
        static const AkUniqueID PLAYERMASTER = 3538689948U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID REVERBS = 3545700988U;
        static const AkUniqueID SFX = 393239870U;
        static const AkUniqueID UNDERWATER = 2213237662U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
