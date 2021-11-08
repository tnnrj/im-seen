export enum ChartType {
  None = 'None',
  Bar = 'Bar',
  Line = 'Line',
  Pie = 'Pie', // only probably should be used in student view for now
  BubbleCloud = 'BubbleCloud'
}

export enum DashLayout {
  None = 'None',
  One = 'One',
  TwoLR = 'TwoLR',
  TwoTB = 'TwoTB',
  ThreeOneTB = 'ThreeOneTB',
  ThreeOneLR = 'ThreeOneLR',
  ThreeTwoTB = 'ThreeTwoTB',
  ThreeTwoLR = 'ThreeTwoLR',
  FourTB = 'FourTB',
  FourOneTB = 'FourOneTB',
  FourOneLR = 'FourOneLR',
  FourThreeTB = 'FourThreeTB',
  FourThreeLR = 'FourThreeLR',
  FiveTwoTB = 'FiveTwoTB',
  FiveTwoLR = 'FiveTwoLR',
  FiveThreeTB = 'FiveThreeTB',
  FiveThreeLR = 'FiveThreeLR'
}

export enum UserRole {
  Administrator = 'Administrator',
  PrimaryActor = 'PrimaryActor',
  SupportingActor = 'SupportingActor',
  Observer = 'Observer'
}

export enum ObservationStatus {
  New = 'New',
  Viewed = 'Viewed',
  Assigned = 'Assigned',
  ActionTaken = 'ActionTaken',
  Closed = 'Closed'
}