using System.Web;
using HomeBoxLanding.Api.Features.Spotify.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Spotify;

public class SpotifyService
{
    private const string Songs = "Obywatel G.C. - NIE PYTAJ O POLSKĘ\", \"Lao Che - WOJENKA\", \"Maanam - KRAKOWSKI SPLEEN\", \"Kult - ARAHJA\", \"Czesław Niemen - DZIWNY JEST TEN ŚWIAT\", \"Kult - POLSKA\", \"Perfect - AUTOBIOGRAFIA\", \"Myslovitz - DŁUGOŚĆ DŹWIĘKU SAMOTNOŚCI\", \"Fisz Emade Tworzywo - OK, BOOMER!\", \"Męskie Granie Orkiestra 2021 - I CIEBIE TEŻ, BARDZO\", \"Andrzej Zaucha - BYŁAŚ SERCA BICIEM\", \"Dżem - LIST DO M.\", \"Chłopcy z Placu Broni - KOCHAM WOLNOŚĆ\", \"Kazik - 12 GROSZY\", \"Sztywny Pal Azji - WIEŻA RADOŚCI, WIEŻA SAMOTNOŚCI\", \"Marek Grechuta & Anawa - DNI, KTÓRYCH NIE ZNAMY\", \"Republika - BIAŁA FLAGA\", \"Zbigniew Wodecki - LUBIĘ WRACAĆ TAM GDZIE BYŁEM\", \"Edmund Fetting - NIM WSTANIE DZIEŃ (Z FILMU \"PRAWO I PIĘŚĆ\")\", \"Aya RL - SKÓRA\", \"Kaśka Sochacka - NIEBO BYŁO RÓŻOWE\", \"Daab - W MOIM OGRODZIE\", \"Martyna Jakubowicz - W DOMACH Z BETONU NIE MA WOLNEJ MIŁOŚCI\", \"Hey & Edyta Bartosiewicz - MOJA I TWOJA NADZIEJA\", \"Lao Che - HYDROPIEKŁOWSTĄPIENIE\", \"Kobranocka - KOCHAM CIĘ JAK IRLANDIĘ\", \"Czesław Niemen - SEN O WARSZAWIE\", \"Budka Suflera - JOLKA, JOLKA, PAMIĘTASZ\", \"Hey - TEKSAŃSKI\", \"Grażyna Łobaszewska - CZAS NAS UCZY POGODY\", \"Raz, Dwa, Trzy - TRUDNO NIE WIERZYĆ W NIC\", \"Rezerwat - ZAOPIEKUJ SIĘ MNĄ\", \"Kult - DO ANI\", \"Breakout - MODLITWA\", \"Daria Zawiałow - JESZCZE W ZIELONE GRAMY\", \"Krystyna Prońko - JESTEŚ LEKIEM NA CAŁE ZŁO\", \"Happysad - ZANIM PÓJDĘ\", \"Lombard - PRZEŻYJ TO SAM\", \"Maanam - KOCHAM CIĘ KOCHANIE MOJE\", \"Męskie Granie Orkiestra 2018 - POCZĄTEK\", \"KSU - MOJE BIESZCZADY\", \"Tadeusz Woźniak - ZEGARMISTRZ ŚWIATŁA\", \"Dżem - WEHIKUŁ CZASU\", \"Paktofonika - JESTEM BOGIEM\", \"Franek Kimono - KING BRUCE LEE KARATE MISTRZ\", \"Obywatel G.C. - TAK, TAK... TO JA\", \"Róże Europy i Edyta Bartosiewicz - JEDWAB\", \"Grzegorz Turnau - BRACKA\", \"Turbo - DOROSŁE DZIECI\", \"Kazik - TWÓJ BÓL JEST LEPSZY NIŻ MÓJ\", \"Pidżama Porno - NIKT TAK PIĘKNIE NIE MÓWIŁ, ŻE SIĘ BOI MIŁOŚCI\", \"TSA - 51\", \"Republika - ODCHODZĄC\", \"Klaus Mitffoch - JEZU JAK SIĘ CIESZĘ\", \"Stanisław Soyka - TOLERANCJA / NA MIŁY BÓG\", \"Brygada Kryzys - TO CO CZUJESZ, TO CO WIESZ\", \"Tilt - JESZCZE BĘDZIE PRZEPIĘKNIE\", \"Kult - BARANEK\", \"Anna Jantar - NIC NIE MOŻE WIECZNIE TRWAĆ\", \"Kwiat Jabłoni - MOGŁO BYĆ NIC\", \"Małgorzata Ostrowska - MELUZYNA\", \"Krystyna Janda - NA ZAKRĘCIE\", \"Dawid Podsiadło - NIEZNAJOMY\", \"Budka Suflera - JEST TAKI SAMOTNY DOM\", \"Jerzy Stuhr - ŚPIEWAĆ KAŻDY MOŻE\", \"Andrzej Zaucha - C'EST LA VIE - PARYŻ Z POCZTÓWKI\", \"Artur Andrus - PIŁEM W SPALE, SPAŁEM W PILE\", \"Hey - ZAZDROŚĆ\", \"Dżem - SEN O VICTORII\", \"Kaczmarski, Gintrowski, Łapiński - MURY\", \"Krzysztof Zalewski - MIŁOŚĆ, MIŁOŚĆ\", \"Krystyna Prońko - PSALM STOJĄCYCH W KOLEJCE\", \"T.Love - WARSZAWA\", \"Luxtorpeda - AUTYSTYCZNY\", \"Hurt - ZAŁOGA G\", \"Edyta Bartosiewicz - OSTATNI\", \"Dżem - DO KOŁYSKI\", \"Katarzyna Nosowska - JEŚLI WIESZ CO CHCĘ POWIEDZIEĆ\", \"O.N.A. - KIEDY POWIEM SOBIE DOŚĆ\", \"Jacek Kaczmarski - OBŁAWA\", \"Jacek Kaczmarski - NASZA KLASA\", \"Coma - LOS, CEBULA I KROKODYLE ŁZY\", \"Maanam - CYKADY NA CYKLADACH\", \"Perfect - NIE PŁACZ EWKA\", \"Breakout - KIEDY BYŁEM MAŁYM CHŁOPCEM\", \"Stare Dobre Małżeństwo - BIESZCZADZKIE ANIOŁY\", \"Grzegorz Turnau - NAPRAWDĘ NIE DZIEJE SIĘ NIC\", \"Voo Voo - NIM STANIE SIĘ TAK, JAK GDYBY NIGDY NIC\", \"Zbigniew Wodecki with Mitch & Mitch Orchestra And Choir - RZUĆ TO WSZYSTKO CO ZŁE\", \"Artur Rojek - BEKSA\", \"Zbigniew Wodecki - ZACZNIJ OD BACHA\", \"Coma - SPADAM\", \"Perfect - NIEPOKONANI\", \"Stare Dobre Małżeństwo - CZARNY BLUES O CZWARTEJ NAD RANEM\", \"T.Love - KING\", \"Varius Manx - PIOSENKA KSIĘŻYCOWA\", \"Myslovitz i Marek Grechuta - KRAKÓW\", \"Lombard - SZKLANA POGODA\", \"Bogusław Mec - JEJ PORTRET\", \"Edyta Geppert - OCH ŻYCIE, KOCHAM CIĘ NAD ŻYCIE\", \"Artur Rojek - SYRENY\", \"Lady Pank - ZAWSZE TAM GDZIE TY\", \"Budka Suflera - CIEŃ WIELKIEJ GÓRY\", \"De Mono - KOCHAĆ INACZEJ\", \"Kortez - ZOSTAŃ\", \"Kaśka Sochacka - WIŚNIA\", \"Sosnowski - PYŁ\", \"Dawid Podsiadło - MAŁOMIASTECZKOWY\", \"2 plus 1 - CHODŹ, POMALUJ MÓJ ŚWIAT\", \"Chłopcy z Placu Broni - KOCHAM CIĘ\", \"Akurat - DO PROSTEGO CZŁOWIEKA\", \"Dżem - WHISKY\", \"Stan Borys - JASKÓŁKA UWIĘZIONA\", \"Czesław Niemen - WSPOMNIENIE\", \"Wojciech Młynarski - JESZCZE W ZIELONE GRAMY\", \"Bajm - CO MI PANIE DASZ\", \"Kaliber 44 - PLUS I MINUS\", \"Coma - LESZEK ŻUKOWSKI\", \"Marek Grechuta & Anawa - KOROWÓD\", \"Myslovitz - CHCIAŁBYM UMRZEĆ Z MIŁOŚCI\", \"Renata Przemyk & Kasia Nosowska - KOCHANA\", \"Mr. Z'oob - KAWAŁEK PODŁOGI\", \"Elektryczne Gitary - DZIECI\", \"Hey - LIST\", \"Lech Janerka - ROWER\", \"Kortez - Z IMBIREM\", \"Strachy na Lachy - DZIEŃ DOBRY, KOCHAM CIĘ\", \"Lady Pank - MNIEJ NIŻ ZERO\", \"Budka Suflera - SEN O DOLINIE\", \"Kult - HEJ, CZY NIE WIECIE\", \"Blenders - CIĄGNIK\", \"Republika - TELEFONY\", \"Lady Pank - SZTUKA LATANIA\", \"Strachy na Lachy - PIŁA TANGO\", \"Mikromusic - TAK MI SIĘ NIE CHCE\", \"IRA - NADZIEJA\", \"Czesław Niemen - POD PAPUGAMI\", \"Hey - MIMO WSZYSTKO\", \"Ralph Kaminski - KOSMICZNE ENERGIE\", \"Krzysztof Komeda - ROSEMARY'S BABY\", \"Marek Biliński - UCIECZKA Z TROPIKU\", \"Kobranocka - I NIKOMU NIE WOLNO SIĘ Z TEGO ŚMIAĆ\", \"Kazik - SPALAM SIĘ\", \"Republika - ZAPYTAJ MNIE CZY CIĘ KOCHAM\", \"Perfect - CHCEMY BYĆ SOBĄ\", \"Pablopavo i Ludziki - KARWOSKI\", \"Świetliki i Linda - FILANDIA\", \"Bluszcz - LAMPARTY\", \"Ewa Bem - SERCE TO JEST MUZYK\", \"Akurat - LUBIĘ MÓWIĆ Z TOBĄ\", \"Maanam - SZAŁ NIEBIESKICH CIAŁ\", \"Marek Grechuta & Anawa - ŚWIECIE NASZ\", \"Budka Suflera - NOC KOMETY\", \"Taco Hemingway - NASTĘPNA STACJA\", \"Lady Pank - MARCHEWKOWE POLE\", \"Basia Stępniak-Wilk i Grzegorz Turnau - BOMBONIERKA\", \"Izabela Trojanowska - WSZYSTKO CZEGO DZIŚ CHCĘ\", \"Tilt - MÓWIĘ CI ŻE\", \"Formacja Nieżywych Schabuff - KLUB WESOŁEGO SZAMPANA\", \"Elektryczne Gitary - CZŁOWIEK Z LIŚCIEM\", \"Urszula - DMUCHAWCE, LATAWCE, WIATR\", \"Maria Peszek - SORRY POLSKO\", \"Kaczmarski, Gintrowski, Łapiński - MODLITWA O WSCHODZIE SŁOŃCA\", \"Fisz Emade Tworzywo - ZA MAŁO CZASU\", \"Edyta Geppert - JAKA RÓŻA, TAKI CIERŃ\", \"Grzegorz Turnau - MIĘDZY CISZĄ A CISZĄ\", \"Maryla Rodowicz - NIECH ŻYJE BAL\", \"Lady Pank - KRYZYSOWA NARZECZONA\", \"Wilki - ELI LAMA SABACHTANI\", \"Król - Z TOBĄ / DO DOMU\", \"Ørganek - FOTOGRAF BROK\", \"Big Cyc - BERLIN ZACHODNI\", \"Alicja Majewska - ODKRYJEMY MIŁOŚĆ NIEZNANĄ\", \"Raz, Dwa, Trzy - POD NIEBEM\", \"Renata Przemyk - BABĘ ZESŁAŁ BÓG\", \"Raz, Dwa, Trzy - I TAK WARTO ŻYĆ\", \"Maanam - BOSKIE BUENOS\", \"Kortez - HEJ WY\", \"Edyta Bartosiewicz - SEN\", \"Muchy - SZARORÓŻOWE\", \"Kombi - SŁODKIEGO MIŁEGO ŻYCIA\", \"Armia - NIEZWYCIĘŻONY\", \"Męskie Granie Orkiestra 2016 - WATAHA\", \"Ørganek - MISSISSIPPI W OGNIU\", \"Król - TE SMAKI I ZAPACHY\", \"Bajm - DWA SERCA, DWA SMUTKI\", \"Ania Dąbrowska - CHARLIE, CHARLIE\", \"Stare Dobre Małżeństwo - Z NIM BĘDZIESZ SZCZĘŚLIWSZA\", \"Fisz Emade Tworzywo feat. Justyna Święs - ŚLADY\", \"Muniek Staszczyk - POLA\", \"Grzegorz Turnau - CICHOSZA\", \"Dżem - CZERWONY JAK CEGŁA\", \"Dżamble - WYMYŚLIŁEM CIEBIE\", \"Lao Che - NIE RAJ\", \"Brodka - GRANDA\", \"Perfect - NIEWIELE CI MOGĘ DAĆ\", \"Kult - CELINA\", \"Kult - GDY NIE MA DZIECI\", \"Czerwone Gitary - PŁONĄ GÓRY, PŁONĄ LASY\", \"Maanam - PO PROSTU BĄDŹ\", \"Męskie Granie Orkiestra 2019 - SOBIE I WAM\", \"Oddział Zamknięty - OBUDŹ SIĘ\", \"Lao Che - KAPITAN POLSKA\", \"Klaus Mitffoch - STRZEŻ SIĘ TYCH MIEJSC\", \"Taco Hemingway - DESZCZ NA BETONIE\", \"Grzegorz Markowski - BALLADA 07 (Z SERIALU TV \"07 ZGŁOŚ SIĘ\")\", \"Chłopcy z Placu Broni - O! ELA\", \"Kazik & Yugoton - MALCZIKI\", \"Krzysztof Krawczyk i Edyta Bartosiewicz - TRUDNO TAK (RAZEM BYĆ NAM ZE SOBĄ...)\", \"Domowe Melodie - GRAŻKA\", \"Bajm - BIAŁA ARMIA\", \"Golden Life - 24.11.94\", \"Lombard - GOŁĘBI PUCH\", \"Republika - RAZ NA MILION LAT\", \"Maanam - LIPSTICK ON THE GLASS\", \"Hanna Banaszak - W MOIM MAGICZNYM DOMU\", \"Alicja Majewska - JESZCZE SIĘ TAM ŻAGIEL BIELI\", \"Zdzisława Sośnicka - ALEJA GWIAZD\", \"Budka Suflera - ZA OSTATNI GROSZ\", \"Kult - KREW BOGA\", \"Marek Grechuta & Anawa - OCALIĆ OD ZAPOMNIENIA\", \"Magda Umer - KONCERT JESIENNY NA DWA ŚWIERSZCZE I WIATR W KOMINIE\", \"Lech Janerka - KONSTYTUCJE\", \"Wilki - SON OF THE BLUE SKY\", \"Kasia Kowalska - A TO CO MAM\", \"Kabaret Starszych Panów - PIOSENKA JEST DOBRA NA WSZYSTKO\", \"Illusion - NÓŻ\", \"Anna Maria Jopek & Pat Metheny - TAM, GDZIE NIE SIĘGA WZROK\", \"Banda i Wanda - HI-FI\", \"Andrzej Kurylewicz - TEMAT SERIALU \"POLSKIE DROGI\"\", \"Kwiat Jabłoni - DZIŚ PÓŹNO PÓJDĘ SPAĆ\", \"Edyta Bartosiewicz - JENNY\", \"Jonasz Kofta - PAMIĘTAJCIE O OGRODACH\", \"Brodka - VARSOVIE\", \"Cool Kids Of Death - BUTELKI Z BENZYNĄ I KAMIENIE\", \"Kult - PO CO WOLNOŚĆ\", \"Hanna Banaszak - SAMBA PRZED ROZSTANIEM\", \"2 plus 1 - WINDĄ DO NIEBA\", \"Fisz Emade Tworzywo - DWA OGNIE\", \"Voo Voo - GDYBYM\", \"Brodka - HORSES\", \"Zbigniew Zamachowski i Grupa MoCarta - KOBIETY JAK TE KWIATY\", \"Skubas - NIE MAM DLA CIEBIE MIŁOŚCI\", \"Maanam - WYJĄTKOWO ZIMNY MAJ\", \"IRA - MÓJ DOM\", \"Obywatel G.C. - PARYŻ - MOSKWA 17.15\", \"Perfect - KOŁYSANKA DLA NIEZNAJOMEJ\", \"Kaśka Sochacka - CICHE DNI\", \"Republika - MOJA KREW\", \"Zbigniew Wodecki i Zdzisława Sośnicka - Z TOBĄ CHCĘ OGLĄDAĆ ŚWIAT\", \"TSA - TRZY ZAPAŁKI\", \"Krzysztof Zalewski - PTAKI (MTV UNPLUGGED)\", \"Kayah & Bregović - BYŁAM RÓŻĄ\", \"De Mono - STATKI NA NIEBIE\", \"Strachy na Lachy - ŻYJĘ W KRAJU\", \"Dezerter - SPYTAJ MILICJANTA\", \"Anna Jantar - TYLE SŁOŃCA W CAŁYM MIEŚCIE\", \"Halina Frąckowiak - PAPIEROWY KSIĘŻYC\", \"Bitamina - NIECH NO TYLKO ZAKWITNĄ JABŁONIE\", \"GrubSon - NA SZCZYCIE\", \"Kult - CZARNE SŁOŃCA\", \"Republika - ŚMIERĆ W BIKINI\", \"Ewa Demarczyk - KARUZELA Z MADONNAMI\", \"Anna Maria Jopek - JA WYSIADAM\", \"Dżem - MODLITWA III - POZWÓL MI\", \"Kult - DZIEWCZYNA BEZ ZĘBA NA PRZEDZIE\", \"Anita Lipnicka - I WSZYSTKO SIĘ MOŻE ZDARZYĆ\", \"Myslovitz - SCENARIUSZ DLA MOICH SĄSIADÓW\", \"Maanam - LUCCIOLA\", \"Brodka & A_GIM - WSZYSTKO CZEGO DZIŚ CHCĘ\", \"Varius Manx - ZANIM ZROZUMIESZ\", \"Kult - LEWE LEWE LOFF\", \"Czesław Śpiewa - MASZYNKA DO ŚWIERKANIA\", \"Czesław Niemen - BEMA PAMIĘCI ŻAŁOBNY - RAPSOD\", \"Wojciech Młynarski - JESTEŚMY NA WCZASACH\", \"Raz, Dwa, Trzy - W WIELKIM MIEŚCIE\", \"Krzysztof Zalewski - POLSKO\", \"Lady Pank - WCIĄŻ BARDZIEJ OBCY\", \"Czesław Niemen - JEDNEGO SERCA\", \"Pidżama Porno - EZOTERYCZNY POZNAŃ\", \"Budka Suflera - NIE WIERZ NIGDY KOBIECIE\", \"Czerwony Tulipan - JEDYNE CO MAM\", \"The Dumplings - KOCHAM BYĆ Z TOBĄ\", \"Kortez - STARE DRZEWA\", \"Sidney Polak - OTWIERAM WINO\", \"KAT - ŁZA DLA CIENIÓW MINIONYCH\", \"T.Love - TO WYCHOWANIE\", \"Krystyna Prońko - MAŁE TĘSKNOTY\", \"Myslovitz - DLA CIEBIE\", \"Kazik & Edyta Bartosiewicz - CZTERY POKOJE\", \"Raz, Dwa, Trzy - JUTRO MOŻEMY BYĆ SZCZĘŚLIWI\", \"Czerwone Gitary - BIAŁY KRZYŻ\", \"Grzegorz Turnau - ZNÓW WĘDRUJEMY\", \"Michał Lorenc - TANIEC ELENY\", \"Czerwone Gitary - KWIATY WE WŁOSACH\", \"Stanisław Soyka - CUD NIEPAMIĘCI\", \"Kapitan Nemo - TWOJA LORELEI\", \"Męskie Granie Orkiestra 2020 - PŁONĄ GÓRY, PŁONĄ LASY\", \"T.Love - IV LICEUM\", \"Myslovitz - PEGGY BROWN\", \"Kazik na Żywo - PLAMY NA SŁOŃCU\", \"Kaśka Sochacka & Vito Bambino - BOJĘ SIĘ O CIEBIE\", \"Dawid Podsiadło - TRÓJKĄTY I KWADRATY\", \"Maryla Rodowicz - MAŁGOŚKA\", \"Kult - PIOSENKA MŁODYCH WIOŚLARZY\", \"T.Love - LUCY PHERE\", \"Seweryn Krajewski - UCIEKAJ MOJE SERCE\", \"Budka Suflera - CZAS OŁOWIU\", \"Maanam - TO TYLKO TANGO\", \"Myslovitz - CHŁOPCY\", \"Kayah & Bregović - ŚPIJ KOCHANIE, ŚPIJ\", \"Wojciech Młynarski - RÓBMY SWOJE\", \"Sidney Polak - CHOMICZÓWKA\", \"Marek Grechuta & Anawa - WIOSNA, ACH TO TY\", \"Andrzej Sikorowski & Grzegorz Turnau - NIE PRZENOŚCIE NAM STOLICY DO KRAKOWA\", \"Kwiat Jabłoni - BUKA\", \"Republika - MAMONA\", \"Dżem - PAW\", \"Ørganek - NIEMIŁOŚĆ\", \"Houk - TRANSMISSION INTO YOUR HEART\", \"Krystyna Prońko - DESZCZ W CISNEJ\", \"Mrozu - ZŁOTO\", \"Mieczysław Fogg - TO OSTATNIA NIEDZIELA\", \"Myslovitz - W DESZCZU MALEŃKICH ŻÓŁTYCH KWIATÓW\", \"Fisz Emade Tworzywo - BIEGNIJ DALEJ SAM\", \"Zakopower - BOSO\", \"Pogodno - ORKIESTRA\", \"Riverside - RIVER DOWN BELOW\", \"Elektryczne Gitary - JESTEM Z MIASTA\", \"Marek Jackowski - OPRÓCZ\", \"Dżem - PARTYZANT\", \"Męskie Granie Orkiestra 2014 - ELEKTRYCZNY\", \"Piotr Bukartyk - KOBIETY JAK TE KWIATY\", \"Firebirds - HARRY\", \"Dawid Podsiadło - W DOBRĄ STRONĘ\", \"Riverside - THE DEPTH OF SELF-DELUSION\", \"Perfect - OBJAZDOWE NIEME KINO\", \"Krzysztof Krawczyk - BO JESTEŚ TY\", \"Coma - ZAPRZEPASZCZONE SIŁY WIELKIEJ ARMII ŚWIĘTYCH ZNAKÓW\", \"Emigranci - NA FALOCHRONIE\", \"Karaś / Rogucki - KILKA WESTCHNIEŃ\", \"Lombard - ADRIATYK, OCEAN GORĄCY\", \"Anna Maria Jopek - ALE JESTEM\", \"Kasia Kowalska - JAK RZECZ\", \"Maanam - SIE ŚCIEMNIA\", \"Oddział Zamknięty - ANDZIA I JA\", \"Kabaret Starszych Panów - JEŻELI KOCHAĆ TO NIE INDYWIDUALNIE\", \"Magda Umer - OCZY TEJ MAŁEJ\", \"Human - POLSKI\", \"De Press - BO JO CIE KOCHOM\", \"Maryla Rodowicz - ŁATWOPALNI\", \"Dżem - SKAZANY NA BLUESA\", \"Edyta Bartosiewicz - SKŁAMAŁAM\", \"Ewa Demarczyk - GRANDE VALSE BRILLANTE\", \"Oddział Zamknięty - TEN WASZ ŚWIAT\", \"Hey - JA SOWA\", \"Piotr Szczepanik - KOCHAĆ";
    private const string PlaylistId = "0pBFkvFtjydwKq4fLGfcij";
    
    public SpotifyImportSongsResponse GetActivity(SpotifyTestRequest request)
    {
        return new SpotifyImportSongsResponse();
        
        var authToken = GetAuthenticationToken(request.Code);

        if (authToken == null)
            return new SpotifyImportSongsResponse();

        var parsedSongs = Songs.Split("\", \"");

        var spotifyUris = new List<string>();
        
        foreach (var song in parsedSongs.Take(1))
        {
            var potentialUri = SearchForSong(authToken, song);
            
            if (potentialUri is not null)
            {
                spotifyUris.Add(potentialUri);
            }
        }

        var success = InsertSongsIntoPlaylist(authToken, spotifyUris);
        
        return new SpotifyImportSongsResponse
        {
            HasError = !success
        };
    }

    private string? GetAuthenticationToken(string authCode)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Add("Authorization","Basic NDk1ZTllM2FkMDdmNGM1YjlhODU0ZGYxMTVlMmNmNDM6MWRiNTIwZDU5ZDMzNDc0NWFhNDI3ZWQ5OWZiYWNkMzM=");
        
        var parameters = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "authorization_code"),
            new("code", authCode),
            new("redirect_uri", "http://localhost:4200/spotify")
        };
        var result = httpClient.PostAsync($"https://accounts.spotify.com/api/token", new FormUrlEncodedContent(parameters)).Result;
        
        var response = result.Content.ReadAsStringAsync().Result;

        SpotifyTokenResponse? parsedResponse;
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<SpotifyTokenResponse>(response);
        }
        catch (Exception)
        {
            return null;
        }

        return parsedResponse?.AccessToken;
    }

    private string? SearchForSong(string authToken, string song)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {authToken}");
        
        var builder = new UriBuilder("https://api.spotify.com/v1/search")
        {
            Port = -1
        };
        
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["q"] = song;
        query["type"] = "track";
        builder.Query = query.ToString();
        
        var result = httpClient.GetAsync(builder.ToString()).Result;
        
        var response = result.Content.ReadAsStringAsync().Result;

        SpotifySearchResponse? parsedResponse;
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<SpotifySearchResponse>(response);
        }
        catch (Exception)
        {
            return null;
        }

        return parsedResponse?.Tracks?.Items?.FirstOrDefault()?.SpotifyUri;
    }

    private bool InsertSongsIntoPlaylist(string authToken, List<string> spotifyUris)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {authToken}");

        var request = new
        {
            uris = spotifyUris
        };
        
        var result = httpClient.PostAsync($"https://api.spotify.com/v1/playlists/{PlaylistId}/tracks", new StringContent(JsonConvert.SerializeObject(request))).Result;
        
        var response = result.Content.ReadAsStringAsync().Result;

        SpotifyInsertTracksToPlaylistResponse? parsedResponse;
        try
        {
            parsedResponse = JsonConvert.DeserializeObject<SpotifyInsertTracksToPlaylistResponse>(response);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}