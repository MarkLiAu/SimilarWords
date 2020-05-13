import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';

export class WordSearch extends Component {
  displayName = WordSearch.name

  constructor(props) {
    super(props);
      this.state = { wordInput : 'start' };
    }

    WordChanged = (e) => {
            this.setState({
                wordInput: e.target.value
            });
    }

    SubmitSearch = () => {
        fetch('api/Words/' + this.state.wordInput)
            .then(response => response.json())
            .then(data => {
                this.setState({ words: data });
            });
    };

    ExternalDictLink = (name) => {
        return (
            <a href={'https://www.collinsdictionary.com/dictionary/english/' + name} target="_blank">{name}</a>
            );
    }

    ShowListZZZ = (list) => {
        if (typeof (list) === "undefined") return (<div></div>);
        return (
            <div>
                < table className='table' >
                    <thead>
                        <tr>
                            <th>Similar words</th>
                            <th>Frequency</th>
                            <th>Pronounciation</th>
                            <th>Meaning</th>
                        </tr>
                    </thead>
                    <tbody>
                        {list.map(w =>
                            <tr key={w.name}>
                                <td><a href={'https://www.collinsdictionary.com/dictionary/english/'+w.name} target="_blank">{w.name}</a></td>
                                <td>{w.frequency}</td>
                                <td>{w.pronounciation}</td>
                                <td>{w.meaningShort}</td>
                            </tr>
                        )}
                    </tbody>
                </table >
            </div >
        );
    }

    ShowList = (list) => {
        if (typeof (list) === "undefined") return (<div></div>);
        return (
            <Grid fluid>
                {list.map(w =>
                    <Row key={w.name}>
                        <col>
                            hello
                        </col>

                    </Row>
                )}
            </Grid>
        )
    };


    KeyPressed = (event) => {
        if (event.key === 'Enter') {
            this.SubmitSearch();
        }
    }
  render() {
    return (
        <div>
            <h1>Similar Words Search</h1>
            <div>Search word:{this.state.wordInput}</div>
            <input onChange={this.WordChanged} onKeyPress={this.KeyPressed} ></input>  <button type="submit"  onClick={this.SubmitSearch}>Search</button>

            {this.ShowList(this.state.words)}
        </div>
    );
  }
}
