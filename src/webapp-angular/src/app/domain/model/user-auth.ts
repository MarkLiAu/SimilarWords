export class UserPrinciple {
  identityProvider: string | undefined;
  userId: string | undefined;
  userDetails: string | undefined;
  userRoles: string[] | undefined;
}

export class UserInfo {
  clientPrincipal : UserPrinciple | undefined;
}

