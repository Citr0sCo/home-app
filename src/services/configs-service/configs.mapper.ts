import { IConfigs } from './types/configs.type';
import { ISetting } from './types/setting.type';
export class ConfigsMapper {

    public static map(response: any): IConfigs {
        return {
            weatherApiKey: response.WeatherApiKey,
            mapsApiKey: response.MapsApiKey
        };
    }

    public static mapSetting(response: any): ISetting {
        return {
            key: response.Key,
            environmentVariable: response.EnvironmentVariable,
            label: response.Label,
            description: response.Description,
            value: response.Value,
            isSecret: response.IsSecret,
            isConfigured: response.IsConfigured
        };
    }
}