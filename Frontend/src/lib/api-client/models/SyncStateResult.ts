/* tslint:disable */
/* eslint-disable */
/**
 * BoarDo.Server
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { exists, mapValues } from '../runtime';
/**
 * 
 * @export
 * @interface SyncStateResult
 */
export interface SyncStateResult {
    /**
     * 
     * @type {boolean}
     * @memberof SyncStateResult
     */
    syncEnabled?: boolean;
}

/**
 * Check if a given object implements the SyncStateResult interface.
 */
export function instanceOfSyncStateResult(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function SyncStateResultFromJSON(json: any): SyncStateResult {
    return SyncStateResultFromJSONTyped(json, false);
}

export function SyncStateResultFromJSONTyped(json: any, ignoreDiscriminator: boolean): SyncStateResult {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'syncEnabled': !exists(json, 'syncEnabled') ? undefined : json['syncEnabled'],
    };
}

export function SyncStateResultToJSON(value?: SyncStateResult | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'syncEnabled': value.syncEnabled,
    };
}

