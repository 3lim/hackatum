package main

import (
	"fmt"
	"io/ioutil"
	"net/http"
	"strings"

	"github.com/laurent22/ical-go/ical"
)

func main() {
	resp, _ := http.Get("https://campus.tum.de/tumonlinej/ws/termin/ical?pStud=86B0D5AADF940852&pToken=5195E615AC67F588244D57AACEEA6FA0047DAD78531A90455ECCFB6D1C93105E")
	defer resp.Body.Close()
	body, _ := ioutil.ReadAll(resp.Body)
	parser, _ := ical.ParseCalendar(string(body))
	parsed := parser.ChildrenByName("VEVENT")
	lectures := map[string]bool{}
	for ix := range parsed {
		lectures[parsed[ix].PropString("SUMMARY", "none")] = true
	}

	for lecture, _ := range lectures {
		ixBracket := strings.IndexAny(lecture, "(,-")
		if ixBracket != -1 {
			fmt.Println(lecture[:ixBracket])
		} else {
			fmt.Println(lecture)
		}
	}
}
