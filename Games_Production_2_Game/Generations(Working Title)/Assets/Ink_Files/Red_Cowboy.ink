INCLUDE globals.ink


VAR Character = ""
-> NPC_1_Choice_1
=== NPC_1_Choice_1 ===
Howdy Partner, you're lookin kinda rough #speaker:Red_Cowboy #portrait:Red_Cowboy #audio:Red_Cowboy
Do you want a honest gentleman such as myself to help you out there?

- Give me a minute to think. #speaker:Talos #portrait:Talos #audio:Talos
+[Yes]
->NPC_1_result_1
+[No]
-> NPC_1_result_2


=== NPC_1_result_1 ===
~ Character = "Red_Guy"
Brother I wont let you down! #JoinParty:Red_Cowboy #PartyMember:Red_Guy #speaker:Red_Cowboy #portrait:Red_Cowboy #audio:Red_Cowboy
-> END

=== NPC_1_result_2 ===
~CanJoinParty = 0
Well maybe next time :( #speaker:Red_Cowboy #portrait:Red_Cowboy #audio:Red_Cowboy
-> END