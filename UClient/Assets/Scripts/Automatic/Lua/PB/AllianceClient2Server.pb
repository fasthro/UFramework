
¯I
AllianceClient2Server.proto
NBSGame.PBCommon.proto"(
C2S_CreateAlliance
name (	Rname"
S2C_CreateAlliance"5
C2S_GetAllianceInfo

allianceId (R
allianceId"Ÿ
S2C_GetAllianceInfo
name (	Rname&
leaderPlayerId (RleaderPlayerId

leaderName (	R
leaderName0
diplomatistPlayerId (RdiplomatistPlayerId(
diplomatistName (	RdiplomatistName
level (Rlevel 
memberCount (RmemberCount
	memberMax (R	memberMax
region	 (Rregion
power
 (Rpower
	townCount (R	townCount
notice (	Rnotice
resMax (RresMax
res1 (Rres1
res2 (Rres2
res3 (Rres3
res4 (Rres4
score (Rscore
scoreMax (RscoreMax
relation (Rrelation
isReleve (RisReleve"3
C2S_ApplyAlliance

allianceId (R
allianceId"
S2C_ApplyAlliance"
C2S_ApplyAllianceList"é
S2C_ApplyAllianceListA
allianceInfo (2.NBSGame.PB.ApplyAllianceInfoRallianceInfo2
joinAllianceCoolTime (RjoinAllianceCoolTime"É
ApplyAllianceInfo

allianceId (R
allianceId
name (	Rname
level (Rlevel 
memberCount (RmemberCount
	memberMax (R	memberMax
region (Rregion
distance (Rdistance
isJoin (RisJoin
isApply	 (RisApply"
C2S_ApplyAlliancePlayerList"z
S2C_ApplyAlliancePlayerListC

playerInfo (2#.NBSGame.PB.ApplyAlliancePlayerInfoR
playerInfo
isOpen (RisOpen"≠
ApplyAlliancePlayerInfo
playerId (RplayerId
name (	Rname
power (Rpower
region (Rregion
isRead (RisRead
	applyTime (R	applyTime"R
C2S_ApplyAllianceProcessor
playerId (RplayerId
isAgree (RisAgree"
S2C_ApplyAllianceProcessor"0
C2S_InviteAlliance
playerId (RplayerId"
S2C_InviteAlliance"
C2S_InviteAllianceList"h
S2C_InviteAllianceListN
inviteAllianceInfo (2.NBSGame.PB.InviteAllianceInfoRinviteAllianceInfo" 
InviteAllianceInfo

allianceId (R
allianceId
name (	Rname
level (Rlevel 
memberCount (RmemberCount
	memberMax (R	memberMax*
invitePlayerName (	RinvitePlayerName"W
C2S_InviteAllianceProcessor

allianceId (R
allianceId
isAgree (RisAgree"
S2C_InviteAllianceProcessor"
C2S_ClearInviteAlliance"
S2C_ClearInviteAlliance"
C2S_GetMemberList"u
S2C_GetMemberList6

memberInfo (2.NBSGame.PB.MemberInfoR
memberInfo(
demiseTimestamp (RdemiseTimestamp"ê

MemberInfo
playerId (RplayerId
name (	Rname
avatar (Ravatar
power (Rpower
region (Rregion
cellId (RcellId
position (Rposition"
contribution (Rcontribution*
contributionWeek	 (RcontributionWeek
score
 (Rscore
	scoreWeek (R	scoreWeek
donate (Rdonate

donateWeek (R
donateWeek
	isCapture (R	isCapture"
C2S_GetCaptureList"O
S2C_GetCaptureList9
captureInfo (2.NBSGame.PB.CaptureInfoRcaptureInfo"ô
CaptureInfo
playerId (RplayerId
name (	Rname
region (Rregion
cellId (RcellId*
captureTimestamp (RcaptureTimestamp"
C2S_GetGroupList"G
S2C_GetGroupList3
	groupInfo (2.NBSGame.PB.GroupInfoR	groupInfo"ô
	GroupInfo
groupId (RgroupId
name (	Rname

leaderName (	R
leaderName
count (Rcount
avgScore (RavgScore"
avgScoreWeek (RavgScoreWeek
	avgDonate (R	avgDonate$
avgDonateWeek (RavgDonateWeek$
leaderPlayrId	 (RleaderPlayrId"
C2S_GetAllianceAddition"±
S2C_GetAllianceAddition"
res1addition (Rres1addition"
res2addition (Rres2addition"
res3addition (Rres3addition"
res4addition (Rres4addition&
memberAddition (RmemberAddition*
townRes1addition (RtownRes1addition*
townRes2addition (RtownRes2addition*
townRes3addition (RtownRes3addition*
townRes4addition	 (RtownRes4addition.
townMemberAddition
 (RtownMemberAddition"$
C2S_GetLogList
type (Rtype"9
S2C_GetLogList'
item (2.NBSGame.PB.LogItemRitem"S
LogItem
	timeStamp (R	timeStamp*
text (2.NBSGame.PB.CommonTextRtext"*
C2S_ChangeNotice
notice (	Rnotice"
S2C_ChangeNotice"
C2S_AppointOfficerView"E
S2C_AppointOfficerView+
item (2.NBSGame.PB.OfficerItemRitem"=
OfficerItem
name (	Rname
position (Rposition"L
C2S_AppointOfficer
playerId (RplayerId
position (Rposition"
S2C_AppointOfficer"/
C2S_RecallOfficer
playerId (RplayerId"
S2C_RecallOfficer"0
C2S_AllianceDemise
playerId (RplayerId"
S2C_AllianceDemise"
C2S_ConncelAllianceDemise"
S2C_ConncelAllianceDemise",
C2S_KickMember
playerId (RplayerId"
S2C_KickMember"
C2S_QuitAlliance"
S2C_QuitAlliance"C
C2S_CreateGroup
name (	Rname
	playerIds (R	playerIds"
S2C_CreateGroup"[
C2S_EditGroup
groupId (RgroupId
name (	Rname
	playerIds (R	playerIds"
S2C_EditGroup"
C2S_GetAppointGroupLeaderList"W
S2C_GetAppointGroupLeaderList6
leader (2.NBSGame.PB.AppointGroupLeaderRleader"`
AppointGroupLeader
playerId (RplayerId
name (	Rname
position (Rposition"N
C2S_AppointGroupLeader
playerId (RplayerId
groupId (RgroupId"
S2C_AppointGroupLeader"1
C2S_RecallGroupLeader
groupId (RgroupId"
S2C_RecallGroupLeader"-
C2S_DissolveGroup
groupId (RgroupId"
S2C_DissolveGroup"0
C2S_GetMoveGroupList
groupId (RgroupId"I
S2C_GetMoveGroupList1
groups (2.NBSGame.PB.MoveGroupInfoRgroups"=
MoveGroupInfo
groupId (RgroupId
name (	Rname"K
C2S_MoveGroup
playerId (RplayerId

newGroupId (R
newGroupId"
S2C_MoveGroup"-
C2S_RemoveGroup
playerId (RplayerId"
S2C_RemoveGroup",
C2S_GetGroupInfo
groupId (RgroupId"c
S2C_GetGroupInfo
name (	Rname;

memberInfo (2.NBSGame.PB.GroupMemberInfoR
memberInfo"ã
GroupMemberInfo
playerId (RplayerId
name (	Rname
score (Rscore
	scoreWeek (R	scoreWeek
power (Rpower"
C2S_GetNotGroupMemberList"X
S2C_GetNotGroupMemberList;

memberInfo (2.NBSGame.PB.GroupMemberInfoR
memberInfo"2
C2S_GetGroupMemberList
groupId (RgroupId"x
S2C_GetGroupMemberList6

memberInfo (2.NBSGame.PB.MemberInfoR
memberInfo&
leaderPlayerId (RleaderPlayerId"
C2S_QuitGroup"
S2C_QuitGroup"9
C2S_CancelApplyAlliance

allianceId (R
allianceId"
S2C_CancelApplyAlliance"-
C2S_SetAllianceOpen
isOpen (RisOpen"
S2C_SetAllianceOpen"
C2S_ApplyAllianceAllReject"
S2C_ApplyAllianceAllReject"
C2S_AllianceRank"∏
S2C_AllianceRank

allianceId (R
allianceId"
allianceName (	RallianceName
rank (Rrank
level (Rlevel 
memberCount (RmemberCount
	townCount (R	townCount
region (Rregion
power (RpowerH
allianceRankInfo	 (2.NBSGame.PB.AllianceRankInfoRallianceRankInfo"Ó
AllianceRankInfo

allianceId (R
allianceId"
allianceName (	RallianceName
rank (Rrank
level (Rlevel 
memberCount (RmemberCount
	townCount (R	townCount
region (Rregion
power (Rpower"_
C2S_AddAllianceFavorite
cellId (RcellId
name (	Rname
content (	Rcontent"
S2C_AddAllianceFavorite"4
C2S_CancelAllianceFavorite
cellId (RcellId"
S2C_CancelAllianceFavorite"
C2S_GetAllianceFavoriteList"U
S2C_GetAllianceFavoriteList6
items (2 .NBSGame.PB.AllianceFavoriteItemRitems"ä
AllianceFavoriteItem
cellId (RcellId
name (	Rname

createTime (R
createTime&
cretatPlayerId (RcretatPlayerId"7
C2S_GetAllianceFavoriteDetail
cellId (RcellId"π
S2C_GetAllianceFavoriteDetail
name (	Rname
content (	Rcontent*
createPlayerName (	RcreatePlayerName&
cretatPlayerId (RcretatPlayerId
cellId (RcellId"
C2S_GetAllianceDonationInfo"≠
S2C_GetAllianceDonationInfo
level (Rlevel"
currentScore (RcurrentScore
	needScore (R	needScore&
allianceResMax (RallianceResMax"
allianceRes1 (RallianceRes1"
allianceRes2 (RallianceRes2"
allianceRes3 (RallianceRes3"
allianceRes4 (RallianceRes4"f
C2S_AllianceDonation
res1 (Rres1
res2 (Rres2
res3 (Rres3
res4 (Rres4"
S2C_AllianceDonation"U
C2S_SetAllianceRelation

allianceId (R
allianceId
relation (Rrelation"
S2C_SetAllianceRelation"
C2S_AllianceRelationList"R
S2C_AllianceRelationList6
items (2 .NBSGame.PB.AllianceRelationItemRitems"˛
AllianceRelationItem

allianceId (R
allianceId
name (	Rname
level (Rlevel
power (Rpower 
memberCount (RmemberCount
region (Rregion
relation (Rrelation0
releveRemainingTime (RreleveRemainingTime"
C2S_AllianceAllyApplyList"T
S2C_AllianceAllyApplyList7
items (2!.NBSGame.PB.AllianceAllyApplyItemRitems"±
AllianceAllyApplyItem

allianceId (R
allianceId
name (	Rname
level (Rlevel
power (Rpower 
memberCount (RmemberCount
region (Rregion"Z
C2S_AllianceAllyApplyProcessor

allianceId (R
allianceId
isAgree (RisAgree" 
S2C_AllianceAllyApplyProcessor"W
C2S_CheckAllianceRelation

allianceId (R
allianceId
relation (Rrelation"
S2C_CheckAllianceRelationBE
$com.rhea.pIII.game.core.cmd.protocolBAllianceClinet2ServerProtocolbproto3