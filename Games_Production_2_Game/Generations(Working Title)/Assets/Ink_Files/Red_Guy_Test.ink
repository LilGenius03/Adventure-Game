INCLUDE globals.ink


VAR Character = ""
-> NPC_1_Choice_1
=== NPC_1_Choice_1 ===
Hello Friend #speaker: #portrait:Red_Guy #audio:Red_Guy
I love the color Red
Can I join your party?

- Give me a minute to think. #speaker:Talos #portrait:Talos #audio:Talos
+[Yes]
->NPC_1_result_1
+[No]
-> NPC_1_result_2


=== NPC_1_result_1 ===
~ Character = "Red_Guy"
I {Character} will not let you down! #JoinParty:Red_Guy #PartyMember:Red_Guy #speaker:Red_Guy #portrait:Red_Guy #audio:Red_Guy 
-> END

=== NPC_1_result_2 ===
~CanJoinParty = 0
Well maybe next time :( #speaker:Red_Guy #portrait:Red_Guy #audio:Red_Guy
-> END