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
 * @interface UrlResult
 */
export interface UrlResult {
    /**
     * 
     * @type {string}
     * @memberof UrlResult
     */
    url?: string | null;
}

/**
 * Check if a given object implements the UrlResult interface.
 */
export function instanceOfUrlResult(value: object): boolean {
    let isInstance = true;

    return isInstance;
}

export function UrlResultFromJSON(json: any): UrlResult {
    return UrlResultFromJSONTyped(json, false);
}

export function UrlResultFromJSONTyped(json: any, ignoreDiscriminator: boolean): UrlResult {
    if ((json === undefined) || (json === null)) {
        return json;
    }
    return {
        
        'url': !exists(json, 'url') ? undefined : json['url'],
    };
}

export function UrlResultToJSON(value?: UrlResult | null): any {
    if (value === undefined) {
        return undefined;
    }
    if (value === null) {
        return null;
    }
    return {
        
        'url': value.url,
    };
}

