package com.eventoweb.ws.relatorios;

import java.util.Map;

/**
 *
 * @author robso
 */
public class ConfiguracoesImpl implements IConfiguracoes {

    private final String mLocalRelatorios;
    private final Map<EnumRelatorio, String> mRelatorios;

    public ConfiguracoesImpl(String localRelatorios, Map<EnumRelatorio, String> relatorios){
        mLocalRelatorios = localRelatorios;
        mRelatorios = relatorios;
    }
    
    @Override
    public String getLocalRelatorios() {
        return mLocalRelatorios;
    }

    @Override
    public Map<EnumRelatorio, String> getRelatorios() {
        return mRelatorios;
    }    
}
