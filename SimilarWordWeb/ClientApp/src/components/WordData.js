import React, { Component } from 'react';

export class WordData extends Component {
    // displayName = WordSearch.name

  constructor(props) {
    super(props);
    this.state = {name:this.props.name, words: [], loading: true };

  }

  render() {
      fetch('api/Words/' + this.state.name)
          .then(response => response.json())
          .then(data => {
              this.setState({ words: data, loading: false });
          });
      return (
          <div>
              props.name={this.props.name}
              state.name={this.state.name}
          <table className='table'>
              <thead>
                  <tr>
                      <th>Similar words</th>
                      <th>Frequency</th>
                      <th>Pronounciation</th>
                      <th>Meaning</th>
                  </tr>
              </thead>
              <tbody>
                  {this.state.words.map(w =>
                      <tr key={w.name}>
                          <td>{w.name}</td>
                          <td>{w.frequency}</td>
                          <td>{w.pronounciation}</td>
                          <td>{w.meaningShort}</td>
                      </tr>
                  )}
              </tbody>
              </table>
              </div>
      );
  }
}
