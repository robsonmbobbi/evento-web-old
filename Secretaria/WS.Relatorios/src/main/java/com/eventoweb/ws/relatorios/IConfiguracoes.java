/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.eventoweb.ws.relatorios;

import java.util.Map;

public interface IConfiguracoes {
    String getLocalRelatorios();
    Map<EnumRelatorio, String> getRelatorios();
}
