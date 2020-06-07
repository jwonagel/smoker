export * from './clientSettings.service';
import { ClientSettingsService } from './clientSettings.service';
export * from './smoker.service';
import { SmokerService } from './smoker.service';
export * from './smokerSettings.service';
import { SmokerSettingsService } from './smokerSettings.service';
export const APIS = [ClientSettingsService, SmokerService, SmokerSettingsService];
