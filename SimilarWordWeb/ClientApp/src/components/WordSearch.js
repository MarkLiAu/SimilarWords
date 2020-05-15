import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';

export class WordSearch extends Component {
  displayName = WordSearch.name

  constructor(props) {
    super(props);
      this.state = { wordInput : 'start', frequency:10000 };
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

    ShowList = (list) => {
        if (typeof (list) === "undefined") return (<div></div>);
        return (
            <Grid fluid>
                <Row className="descriptionDiv">
                    <Col xs={4} md={2}>word</Col>
                    <Col xs={4} md={2}>Frequency</Col>
                    <Col xs={4} md={2}>Pronounciation</Col>
                </Row>
                {list.map(w => {
                    if (Number(w.frequency) > this.state.frequency) return '';
                    return (
                    <Row className="descriptionDiv">
                        <Col xs={4} md={2}><a href={'https://www.collinsdictionary.com/dictionary/english/' + w.name} target="_blank">{w.name}</a></Col>
                        <Col xs={4} md={1}>{w.frequency}</Col>
                        <Col xs={4} md={3}>{w.pronounciation}</Col>
                        <Col  xsOffset={1} xs={11} mdOffset={0} md={6}>{w.meaningShort}</Col>
                        </Row>
                    )})
                }

            </Grid>


        );
    }

    ShowList_table = (list) => {
        if (typeof (list) === "undefined") return (<div></div>);
        return (
            <div>
                {this.state.frequency}
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
                        {list.map(w => {
                            if (Number(w.frequency) > this.state.frequency) return '';
                            return (
                            <tr key={w.name}>
                                <td><a href={'https://www.collinsdictionary.com/dictionary/english/' + w.name} target="_blank">{w.name}</a></td>
                                <td>{w.frequency}</td>
                                <td>{w.pronounciation}</td>
                                <td>{w.meaningShort}</td>
                            </tr>
                        );}
                        )}
                    </tbody>
                </table >
            </div >
        );
    }

    ShowAllChanged = (e) => {
        this.setState({
            frequency: e.target.checked ? 50000 : 10000
        });

    }

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
            <input onChange={this.WordChanged} onKeyPress={this.KeyPressed} ></input>  <button type="submit" onClick={this.SubmitSearch}>Search</button>
            <input className="form-check-input" type="checkbox" id="Showall" name="Showall" onChange={this.ShowAllChanged}></input>
            <label className="form-check-label" for="Showall">Show All</label>

            {this.ShowList(this.state.words)}
        </div>
    );
  }
}
