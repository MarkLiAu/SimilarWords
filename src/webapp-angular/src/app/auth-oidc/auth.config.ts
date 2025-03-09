import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
  issuer: 'https://login.microsoftonline.com/b44fba40-b748-4f17-878f-73c139727a31/v2.0', // OIDC Provider (e.g., Auth0, Azure AD, Keycloak)
  clientId: '5e2d4277-4147-40c4-aeb5-9f282e6eddf9', // Client ID from OIDC provider
  scope: 'openid profile email offline_access api://1450dd42-7830-4eb9-9825-60aa80b3ed13/SimilarWordsApi.Default', // Basic scopes
  responseType: 'code', // Use authorization code flow (more secure)
  redirectUri: window.location.origin, // Redirect after login
  requireHttps: false, // Set to false for local dev if needed
  strictDiscoveryDocumentValidation: false, // Disable strict validation for local dev
  
  disablePKCE: false, // ✅ PKCE must be enabled
  // Enable silent refresh to get new tokens without user interaction
//   silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',
};
