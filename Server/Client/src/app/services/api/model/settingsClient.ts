/**
 * Smoker API
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 *
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { AlertClient } from './alertClient';

export interface SettingsClient {
    settingsId?: string;
    openCloseTreshold?: number;
    isAutoMode?: boolean;
    fireNotifcationTemperatur?: number;
    temperaturUpdateCycleSeconds?: number;
    lastSettingsUpdate?: Date;
    lastSettingsUpdateUser?: string;
    lastSettingsActivation?: Date;
    alerts?: Array<AlertClient>;
}
