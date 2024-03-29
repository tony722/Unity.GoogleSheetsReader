﻿using AET.Unity.SimplSharp;
using System;
using System.Collections.Generic;
using System.Collections;

namespace AET.Unity.GoogleSheetsReader {
  public class Sections : IEnumerable<Section> {
    private readonly IndexedDictionary<string, Section> sections = new IndexedDictionary<string, Section>(StringComparer.OrdinalIgnoreCase);

    public Section this[string name] { get { return sections[name]; } }
    public Section this[int index] {  get { return sections[index]; } }

    public void Add(Section section) {
      sections.Add(section.Name, section);
    }

    public void Add(string name, Section section) {
      sections.Add(name, section);
    }

    public Section Add(IList<string> row) {
      var section = new Section(row);
      sections.Add(section.Name, section);
      return section;
    }

    public int Count {  get { return sections.Count; } }

    public bool TryGet(string name, out Section section) {
      return sections.TryGetValue(name, out section);
    }


    #region IEnumerable<Section> Members

    IEnumerator<Section> IEnumerable<Section>.GetEnumerator() {
      return sections.Values.GetEnumerator();
    }
    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator() {
      return sections.Values.GetEnumerator();
    }

    #endregion
  }
}
