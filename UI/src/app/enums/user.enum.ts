enum UserType {
  Client = 0,
  Support = 1,
  Manager = 2,
}

enum UserStatus {
  Active = 1,
  Deactivated = 0
}
enum TicketStatus {
  New = 0,
  Assigned = 1,
  InProgress = 2,
  Closed = 3
}

export {
  UserType, UserStatus, TicketStatus
}
