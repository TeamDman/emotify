sig Server {
    owners: set User,
    emotes: set Emote,
}

sig User {
    servers: set Server,
}


sig Tag {}
fact {
    // There are no tags not associated to an emote
    #Emote.tags = #Tag
}


sig Name {}

sig Emote {
    uploader: one User,
    tags: set Tag,
    name: one Name,
}


pred someServer {
    #Server > 0
}

run {} for 3
run someServer for 3