/**
 * This file was auto-generated by openapi-typescript.
 * Do not make direct changes to the file.
 */


export interface paths {
  "/api/Auth": {
    get: {
      responses: {
        /** @description Success */
        200: {
          content: {
            "application/json": {
              [key: string]: boolean | undefined;
            };
          };
        };
      };
    };
  };
  "/api/Auth/Connect/Google": {
    get: {
      responses: {
        /** @description Success */
        200: {
          content: {
            "application/json": components["schemas"]["UrlResult"];
          };
        };
      };
    };
  };
  "/api/Auth/Callback/Google": {
    get: {
      parameters: {
        query?: {
          code?: string;
        };
      };
      responses: {
        /** @description Success */
        200: never;
      };
    };
  };
  "/api/Calendar": {
    get: {
      responses: {
        /** @description Success */
        200: {
          content: {
            "application/json": (string)[];
          };
        };
      };
    };
  };
  "/api/Calendar/Sync/{enable}": {
    post: {
      parameters: {
        path: {
          enable: boolean;
        };
      };
      responses: {
        /** @description Success */
        200: never;
      };
    };
  };
  "/api/Calendar/SyncState": {
    get: {
      responses: {
        /** @description Success */
        200: {
          content: {
            "application/json": components["schemas"]["SyncStateResult"];
          };
        };
      };
    };
  };
  "/api/Screen": {
    get: {
      responses: {
        /** @description Success */
        200: never;
      };
    };
  };
}

export type webhooks = Record<string, never>;

export interface components {
  schemas: {
    SyncStateResult: {
      syncEnabled?: boolean;
    };
    UrlResult: {
      url?: string | null;
    };
  };
  responses: never;
  parameters: never;
  requestBodies: never;
  headers: never;
  pathItems: never;
}

export type external = Record<string, never>;

export type operations = Record<string, never>;
