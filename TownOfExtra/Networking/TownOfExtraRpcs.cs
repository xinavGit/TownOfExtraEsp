namespace TownOfExtra.Networking;

public enum TownOfExtraRpcs : uint
{
    SendNotification = 509,
    
    
    TricksterNotifyOfReport = 500,
    TricksterPlaceFakeBody = 501,
    TricksterDestroyFakeBodies = 502,
    CannibalNotifyDead = 503,
    CannibalReviveVictims = 504,
    BrittleTriggerModifier = 505,
    VinculatorEmpowerTeam = 506,
    VultureCleanBody = 507,
    VultureChangeToAmne = 508,
    ConjurerPlaceRock = 510,
    ConjurerAwardSplatAchievement = 511,
    HolographerSyncFakePlayer = 512,
    SendJournalistChat = 513,
    SquidSpillInk = 514,
    SquidDestroyInk = 515,
    RebirthSendPopup = 516,
    BarbarianNotifyTargetDeath = 517,
    CommanderIncreaseAvengeUses = 518,
    ObstructorTriggerObstruct = 519,
}